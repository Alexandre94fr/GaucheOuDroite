// Tool made by Alexandre94.fr (https://github.com/Alexandre94fr/Instantiator)

using System;
using System.Reflection;
using UnityEngine;

namespace InstantiatorPackage
{
    [DefaultExecutionOrder(-50)]
    public class Instantiator : MonoBehaviour
    {
        #region -= Variables =-
        
        public enum InstanceConflictResolutions
        {
            Warning,
            WarningAndPause,
            WarningAndDestroyingDuplicateObject,
            DestroyingDuplicateObject,
            WarningAndDestroyingDuplicateObjectParent,
            DestroyingDuplicateObjectParent,
        }

        /// <summary>
        /// You can modify this variable directly in the script so that any team member using the 
        /// '<see cref="GetInstance"/>' method (not it's overloads) will follow your naming conventions 
        /// for singleton instance variables. </summary>
        static string DEFAULT_SINGLETON_VARIABLE_NAME = "Instance";
        #endregion

        #region -= Methods =-

        void Awake()
        {
            Debug.LogWarning(
                $"<color=yellow>WARNING!</color> The '{nameof(Instantiator)}' script is not meant to be attached to a GameObject.\n" +
                $"Please remove it from the GameObject named '{gameObject.name}'. " +
                $"This is a static utility class, you do not need to attach it to any GameObject for it to function."
            );
        }

        /// <summary> 
        /// If there is <b> no </b> existing Instance, returns the Instance of the specified script type,
        /// else it handles the conflict according to the specified resolution type set. 
        /// 
        /// <para> <b> Note: </b> This function overload will automatically try to find the "Instance" variable.
        /// You can find the targeted name in the '<see cref="DEFAULT_SINGLETON_VARIABLE_NAME"/>' Instantiator's variable. </para>
        /// 
        /// <para> <b> Method utilization example: </b> </para>
        /// <example>
        /// <code> 
        /// public class CLASS_NAME : MonoBehaviour
        /// {
        ///     public static CLASS_NAME Instance;
        /// 
        ///     public void Awake()
        ///     {
        ///         Instance = Instantiator.GetInstance(this, Instantiator.p_instanceConflictResolution.WarningAndPause);
        ///     }
        /// }
        /// </code> </example> </summary>
        /// <typeparam name = "T"> The type of the script to return an Instance of. </typeparam>
        /// <param name = "p_classInstance"> An Instance of the type T, representing the current class Instance. </param>
        /// <param name = "p_instanceConflictResolution"> Defines how to resolve conflicts when multiple instances are detected. </param>
        /// <returns> The Instance of the specified script type.</returns>
        public static T GetInstance<T>(T p_classInstance, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
        {
            #region Getting the "Instance" variable value

            // Get and set the given Class type into a variable
            Type givenClassType = typeof(T);

            // Look for the "Instance" variable who is Public and Static
            FieldInfo instanceVariable = givenClassType.GetField(DEFAULT_SINGLETON_VARIABLE_NAME, BindingFlags.Public | BindingFlags.Static);

            // If we didn't find the so called named "Instance" variable we return an error
            if (instanceVariable == null)
            {
                Debug.LogError($"<color=red>ERROR!</color> The class '{givenClassType}' does not have a public static '{DEFAULT_SINGLETON_VARIABLE_NAME}' variable. Returning null.");
                return null;
            }

            // Get the value of the "Instance" variable
            T instanceVariableValue = (T)instanceVariable.GetValue(null);
            #endregion

            return TryGetInstance(p_classInstance, instanceVariableValue, p_instanceConflictResolution);
        }

        /// <summary> 
        /// If there is <b> no </b> existing Instance, returns the Instance of the specified script type,
        /// else it handles the conflict according to the specified resolution type set. 
        /// 
        /// <para> <b> Method utilization example: </b> </para>
        /// <example>
        /// <code> 
        /// public class CLASS_NAME : MonoBehaviour
        /// {
        ///     public static CLASS_NAME Instance;
        /// 
        ///     public void Awake()
        ///     {
        ///         Instance = Instantiator.GetInstance(this, Instance, Instantiator.p_instanceConflictResolution.WarningAndPause);
        ///     }
        /// }
        /// </code> </example> </summary>
        /// <typeparam name = "T"> The type of the script to return an Instance of. </typeparam>
        /// <param name = "p_classInstance"> An Instance of the type T, representing the current class Instance. </param>
        /// <param name = "p_instanceVariable"> The current value of the singleton instance variable. 
        /// This is used to determine whether an instance already exists or not. </param>
        /// <param name = "p_instanceConflictResolution"> Defines how to resolve conflicts when multiple instances are detected. </param>
        /// <returns> The Instance of the specified script type.</returns>
        public static T GetInstance<T>(T p_classInstance, T p_instanceVariable, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
        {
            return TryGetInstance(p_classInstance, p_instanceVariable, p_instanceConflictResolution);
        }

        static T TryGetInstance<T>(T p_classInstance, T p_instanceVariable, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
        {
            // If an "Instance" value already exists, we handle the conflict
            if (p_instanceVariable != null)
            {
                HandleInstanceConflict(p_classInstance, p_instanceConflictResolution);

                // Returns the value inside the given p_instanceVariable (it's in order to not change the value of the given variable)
                return p_instanceVariable;
            }
            else
            {
                return p_classInstance;
            }
        }

        static void HandleInstanceConflict<T>(T p_classInstance, InstanceConflictResolutions p_instanceConflictResolution) where T : MonoBehaviour
        {
            switch (p_instanceConflictResolution)
            {
                case InstanceConflictResolutions.Warning:
                    HandleWarning(p_classInstance);
                    break;

                case InstanceConflictResolutions.WarningAndPause:
                    HandleWarningAndPause(p_classInstance);
                    break;

                case InstanceConflictResolutions.WarningAndDestroyingDuplicateObject:
                    HandleWarningAndDestroySecond(p_classInstance);
                    break;

                case InstanceConflictResolutions.DestroyingDuplicateObject:
                    HandleDestroySecond(p_classInstance);
                    break;

                case InstanceConflictResolutions.WarningAndDestroyingDuplicateObjectParent:
                    HandleWarningAndDestroySecondParent(p_classInstance);
                    break;

                case InstanceConflictResolutions.DestroyingDuplicateObjectParent:
                    HandleDestroySecondParent(p_classInstance);
                    break;

                default:
                    HandleUnknownResolution(p_instanceConflictResolution);
                    break;
            }
        }

        #region - HandleInstanceConflict sub-methods -

        static void HandleWarning<T>(T p_classInstance) where T : MonoBehaviour
        {
            Debug.LogWarning($"<color=yellow>WARNING!</color> There are multiple '{p_classInstance}' scripts in the scene.");
        }

        static void HandleWarningAndPause<T>(T p_classInstance) where T : MonoBehaviour
        {
            Debug.LogWarning($"<color=yellow>WARNING!</color> There are multiple '{p_classInstance}' scripts in the scene. UNITY IS PAUSED.");
            Debug.Break();
        }

        static void HandleWarningAndDestroySecond<T>(T p_classInstance) where T : MonoBehaviour
        {
            Debug.LogWarning($"<color=yellow>WARNING!</color> There are multiple '{p_classInstance}' scripts in the scene. THE SECOND SCRIPT'S GAMEOBJECT HAS BEEN DESTROYED.");

            Destroy(p_classInstance.gameObject);
        }

        static void HandleDestroySecond<T>(T p_classInstance) where T : MonoBehaviour
        {
            Destroy(p_classInstance.gameObject);
        }

        static void HandleWarningAndDestroySecondParent<T>(T p_classInstance) where T : MonoBehaviour
        {
            Debug.LogWarning($"<color=yellow>WARNING!</color> There are multiple '{p_classInstance}' scripts in the scene. THE SECOND SCRIPT'S GAMEOBJECT PARENT HAS BEEN DESTROYED.");

            if (p_classInstance.transform.parent != null)
                Destroy(p_classInstance.transform.parent.gameObject);
            else
                Debug.LogError($"<color=red>ERROR!</color> The '{p_classInstance.gameObject.name}' GameObject has no parent, meaning we can't destroy it.");
        }

        static void HandleDestroySecondParent<T>(T p_classInstance) where T : MonoBehaviour
        {
            if (p_classInstance.transform.parent != null)
                Destroy(p_classInstance.transform.parent.gameObject);
            else
                Debug.LogError($"<color=red>ERROR!</color> The '{p_classInstance.gameObject.name}' GameObject has no parent, meaning we can't destroy it.");
        }

        static void HandleUnknownResolution(InstanceConflictResolutions p_instanceConflictResolution)
        {
            Debug.LogError($"<color=red>ERROR!</color> The conflict resolution type given '{p_instanceConflictResolution}' is not planned in the switch.");
        }
        #endregion

        #endregion
    }
}

// Tool made by Alexandre94.fr (https://github.com/Alexandre94fr/Instantiator)