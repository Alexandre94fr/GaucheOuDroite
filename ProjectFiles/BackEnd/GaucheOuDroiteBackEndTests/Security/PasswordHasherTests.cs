using GaucheOuDroiteBackEnd.Security;


namespace GaucheOuDroiteBackEndTests.Security
{
    [TestClass]
    public sealed class PasswordHasherTests
    {
        static PasswordHasher _passwordHasher = null!;

        const string EXAMPLE_PASSWORD_1 = "Test1234&";
        const string EXAMPLE_PASSWORD_2 = "ABCD4321*";



        [ClassInitialize]
        public static void ClassInit(TestContext p_context)
        {
            _passwordHasher = new PasswordHasher();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _passwordHasher = null!;
        }


        [DataRow(EXAMPLE_PASSWORD_1)]
        [DataRow(EXAMPLE_PASSWORD_2)]
        [TestMethod]
        public void HashPassword_HashOnePassword_HashedPasswordIsDifferentFromOriginalPassword(string p_password)
        {
            string hashedPassword = _passwordHasher.HashPassword(p_password);

            Assert.AreNotEqual(p_password, hashedPassword);
        }


        [DataRow(EXAMPLE_PASSWORD_1)]
        [DataRow(EXAMPLE_PASSWORD_2)]
        [TestMethod]
        public void VerifyHashedPassword_CorrectPassword_ReturnsTrue(string p_password)
        {
            string hashedPassword = _passwordHasher.HashPassword(p_password);

            bool result = _passwordHasher.VerifyHashedPassword(
                hashedPassword,
                p_password
            );

            Assert.IsTrue(result);
        }


        [DataRow(EXAMPLE_PASSWORD_1)]
        [DataRow(EXAMPLE_PASSWORD_2)]
        [TestMethod]
        public void VerifyHashedPassword_IncorrectPassword_ReturnsFalse(string p_password)
        {
            string hashedPassword = _passwordHasher.HashPassword(p_password);

            string incorrectPassword = p_password + "_Incorrect";

            bool result = _passwordHasher.VerifyHashedPassword(
                hashedPassword,
                incorrectPassword
            );

            Assert.IsFalse(result);
        }


        [DataRow(EXAMPLE_PASSWORD_1)]
        [DataRow(EXAMPLE_PASSWORD_2)]
        [TestMethod]
        public void HashPassword_HashSamePasswordTwice_HashesAreDifferentAndBothVerifyOriginalPassword(string p_password)
        {
            string firstHashedPassword = _passwordHasher.HashPassword(p_password);
            string secondHashedPassword = _passwordHasher.HashPassword(p_password);

            Assert.AreNotEqual(firstHashedPassword, secondHashedPassword);

            bool firstHashVerification = _passwordHasher.VerifyHashedPassword(
                firstHashedPassword,
                p_password
            );

            bool secondHashVerification = _passwordHasher.VerifyHashedPassword(
                secondHashedPassword,
                p_password
            );

            Assert.IsTrue(firstHashVerification);
            Assert.IsTrue(secondHashVerification);
        }
    }
}