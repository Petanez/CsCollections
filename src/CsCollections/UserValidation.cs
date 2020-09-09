namespace CsCollections
{
    public class UserValidation
    {
        public static bool IsYesOrNo(string input)
        {
            if (input == "No" || input == "Yes")
            {
                return true;
            }
            return false;
        }

        public static string ValidateCasing(string userInput)
        {
            string result = "";
            string[] inputArr = userInput.Split(" ");

            if (userInput.Length == 1)
                result = userInput.Substring(0, 1).ToUpper() + userInput.Substring(1).ToLower();
            else if (userInput.Length > 1)
                for (var i = 0; i < inputArr.Length; i++)
                {
                    result += inputArr[i].Substring(0, 1).ToUpper() + inputArr[i].Substring(1).ToLower() + " ";      
                }
            
            return result.Trim();
        }
    }
}