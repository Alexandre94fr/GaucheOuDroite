using System;
using System.Collections.Generic;
using UnityEngine;

namespace VariableCheckerPackage
{
    public static class VariablesChecker
    {
        /// <summary>
        /// A list of checks that will check if the given variable triggers verifications
        /// </summary>
        readonly static List<Func<string, (object variable, string variableName), bool>> DEFAULT_VARIABLE_CHECKS = new()
        {
            IsVariableNullCheck,
        };

        #region Utility methods

        public static string GetCheckErrorMessagePrefix(string p_gameObjectName, string p_variableName)
        {
            return $"<color=red>ERROR !</color> The variable '{p_variableName}' in '{p_gameObjectName}' GameObject";
        }

        public static string GetCheckWarningMessagePrefix(string p_gameObjectName, string p_variableName)
        {
            return $"<color=yellow>WARNING !</color> The variable '{p_variableName}' in '{p_gameObjectName}' GameObject";
        }

        public static bool TryConvertToFloat(object p_input, out float p_result)
        {
            // Note : We use a switch here and not 'Convert.ToSingle()'
            //        because we want to control whitch type can be converted

            switch (p_input)
            {
                case float f:
                    p_result = f;
                    return true;
                case int i:
                    p_result = i;
                    return true;
                case double doubl:
                    p_result = (float)doubl;
                    return true;
                case long l:
                    p_result = l;
                    return true;
                case short s:
                    p_result = s;
                    return true;
                case byte b:
                    p_result = b;
                    return true;
                case decimal decim:
                    p_result = (float)decim;
                    return true;

                default:
                    p_result = 0f;
                    return false;
            }
        }

        #endregion

        #region -= Checks =-

        public static bool IsVariableNullCheck(string p_gameObjectName, (object variable, string variableName) p_variableToCheck)
        {
            if (p_variableToCheck.variable == null || (p_variableToCheck.variable is UnityEngine.Object unityObject && unityObject == null))
            {
                Debug.LogError(
                    $"{GetCheckErrorMessagePrefix(p_gameObjectName, p_variableToCheck.variableName)} is null, " +
                    $"please set it through the Unity inspector, or directly at the variable initialization."
                );
                return true;
            }

            return false;
        }

        public static bool IsNumberVariableUnderZeroCheck(string p_gameObjectName, (object variable, string variableName) p_variableToCheck)
        {
            // Converting the variable value into a number to be able to test it
            if (!TryConvertToFloat(p_variableToCheck.variable, out float floatVariable))
            {
                Debug.LogWarning(
                    $"{GetCheckWarningMessagePrefix(p_gameObjectName, p_variableToCheck.variableName)} is not a number, " +
                    $"so we can't test it throw the {nameof(IsNumberVariableUnderZeroCheck)} check.\n" +

                    $"THE CHECK HAS BEEN IGNORED"
                );
                return false;
            }

            if (floatVariable < 0)
            {
                Debug.LogError(
                    $"{GetCheckErrorMessagePrefix(p_gameObjectName, p_variableToCheck.variableName)} is under zero."
                );
                return true;
            }

            return false;
        }

        public static bool IsNumberVariableEqualZeroCheck(string p_gameObjectName, (object variable, string variableName) p_variableToCheck)
        {
            // Converting the variable value into a number to be able to test it
            if (!TryConvertToFloat(p_variableToCheck.variable, out float floatVariable))
            {
                Debug.LogWarning(
                    $"{GetCheckWarningMessagePrefix(p_gameObjectName, p_variableToCheck.variableName)} is not a number, " +
                    $"so we can't test it throw the {nameof(IsNumberVariableEqualZeroCheck)} check.\n" +

                    $"THE CHECK HAS BEEN IGNORED"
                );
                return false;
            }

            if (floatVariable == 0)
            {
                Debug.LogError(
                    $"{GetCheckErrorMessagePrefix(p_gameObjectName, p_variableToCheck.variableName)} equals zero."
                );
                return true;
            }

            return false;
        }

        // You can add more checks here.
        #endregion

        /// <summary>
        /// Returns if the given variables are valid (by default it's only checking if it's null).
        ///  
        /// <para> ------------------- </para>
        /// 
        /// <para> <b> WARNING : </b> The variable passed are copied, that means that if you past a very big variable this method call will cause lag due to memory management. </para>
        /// 
        /// <para> Sadly we can't pass your variables as references because the keyword 'params' before it is incompatible. </para>
        /// 
        /// <para> However you can use instead the <see cref = "IsVariableValid"/> method. </para>
        /// 
        /// <para> ------------------- </para>
        /// 
        /// <para> <b> Code utilization example n°1 : </b> <code>
        /// 
        /// void Start()
        /// {
        ///     if (!VariablesChecker.AreVariablesValid(name, null,
        ///         (_exampleVariable1, nameof(_exampleVariable1)),
        ///         (_exampleVariable2, nameof(_exampleVariable2)),
        ///         (_exampleVariable3, nameof(_exampleVariable3))
        ///     )) return;
        /// }
        /// </code> </para> 
        /// 
        /// <para> <b> Code utilization example n°2 : </b> </para>
        /// <para> <b> NOTE : </b> Due to a conflict with the XML system (used for code documentation) and the example code below, the 'IN angle brackets, OUT angle brackets' will be replaced by '/ \'. 
        /// But if you see '// Thing' it's just a comment. 
        /// 
        /// <code>
        /// 
        /// int _exampleVariable1 = 42;
        /// string _exampleVariable2 = "Hello world";
        /// bool _exampleVariable3 = true;
        /// 
        /// static readonly List/Func/string, (object variable, string variableName), bool\\ CUSTOM_VARIABLE_CHECKS = new()
        /// {
        ///     VariablesChecker.IsVariableNullCheck,
        ///     IsVariableTestCheck
        ///     // You can add as many methods as you like
        /// };
        /// 
        /// // This method should be 'static'
        /// static bool IsVariableTestCheck(string p_gameObjectName, (object variable, string variableName) p_variableToCheck)
        /// {
        ///     Debug.Log("I'm always returning false for demonstration purpose.");
        /// 
        ///     return false;
        /// }
        /// 
        /// void Start()
        /// {
        ///     if (!VariablesChecker.AreVariablesValid(name, CUSTOM_VARIABLE_CHECKS,
        ///         (_exampleVariable1, nameof(_exampleVariable1)),
        ///         (_exampleVariable2, nameof(_exampleVariable2)),
        ///         (_exampleVariable3, nameof(_exampleVariable3))
        ///     )) return;
        /// }
        /// </code> </para> </summary>
        /// <param name = "p_variableOwnerName"> The variable owner name, can be for example the GameObject name, or the Class name. </param>
        /// <param name = "p_customVariableChecks"> A list of functions (methods that returns something), 
        /// that list of functions correspond to all the checks that the given variable will pass throw, to use default ones just pass 'null'.
        /// You can see the default ones in <see cref = "DEFAULT_VARIABLE_CHECKS"/> </param>
        /// <param name = "p_variablesToCheck"> A params of tuple (an object and a string) </param>
        public static bool AreVariablesValid(string p_variableOwnerName,
            List<Func<string, (object variable, string variableName), bool>> p_customVariableChecks = null,
            params (object variable, string variableName)[] p_variablesToCheck)
        {
            bool areVariablesCorreclySetted = true;

            List<Func<string, (object variable, string variableName), bool>> variableChecks = DEFAULT_VARIABLE_CHECKS;

            // Checking if the user gave customs variable checks
            if (p_customVariableChecks != null)
            {
                variableChecks = p_customVariableChecks;
            }

            // Looping throw each given variables
            for (int i = 0; i < p_variablesToCheck.Length; i++)
            {
                // Doing every checks
                for (int j = 0; j < variableChecks.Count; j++)
                {
                    if (variableChecks[j](p_variableOwnerName, p_variablesToCheck[i]))
                    {
                        areVariablesCorreclySetted = false;
                    }
                }
            }

            return areVariablesCorreclySetted;
        }

        /// <summary>
        /// <b> WARNING ! </b> Not fully usable for now.
        /// <para> Please use '<see cref="AreVariablesValid"/>' instead. </para>
        /// </summary>
        /// <param name = "p_gameObjectName">  </param>
        /// <param name = "p_variable">  </param>
        /// <param name = "p_variableName">  </param>
        /// <param name = "p_customVariableChecks">  </param>
        [Obsolete]
        public static bool IsVariableValid(string p_gameObjectName, ref object p_variable, string p_variableName,
            List<Func<string, (object variable, string variableName), bool>> p_customVariableChecks)
        {
            bool areVariablesCorreclySetted = true;

            List<Func<string, (object variable, string variableName), bool>> variableChecks = DEFAULT_VARIABLE_CHECKS;

            // Checking if the user gave customs variable checks
            if (p_customVariableChecks != null)
            {
                variableChecks = p_customVariableChecks;
            }

            for (int i = 0; i < variableChecks.Count; i++)
            {
                if (variableChecks[i](p_gameObjectName, (p_variable, p_variableName)))
                {
                    areVariablesCorreclySetted = false;
                }
            }

            return areVariablesCorreclySetted;
        }
    }
}