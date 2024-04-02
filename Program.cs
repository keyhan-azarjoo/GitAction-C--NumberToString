using NUnit.Framework;

namespace NumberToTextConvertorNamespace
{

    class Program
    {
        static void Main()
        {
            string Number = "";
            string[] TNumberWord = { "", " thousand ", " million ", " billion " };
            string result = "";
            Number_To_Text_Convertor NumConvertor = new Number_To_Text_Convertor(TNumberWord);
            Console.WriteLine("'q' for exit");
            Console.WriteLine("*********************************************************************");
            // Testing 
            string[] TestNumbers = {"12345","1000000","987654321", "10000000022", "00012000486", "123456789101"};
            for(int i = 0; i<TestNumbers.Length; i++)
            {
                Console.WriteLine("\r\nTest Number is : " + TestNumbers[i]);
                result = NumConvertor.Convertor(TestNumbers[i]);
                if(!result.Contains("Error")){
                    Console.WriteLine("Textual Number is : " + result);
                }
            }
            Console.WriteLine("*********************************************************************");
            while(Number != "q"){
                Console.WriteLine("\r\nPlease enter your Number : ");
                Number = Console.ReadLine();
                result = NumConvertor.Convertor(Number);
                if(!result.Contains("Error") && !result.Contains("Warning")){
                    Console.WriteLine("Textual Number is : " + result);
                }
            }
        }
    }





    // Error Handler
    public interface IErrorHandler
    {
        void WriteError(string Message);
    }
    public class ErrorHandler : IErrorHandler
    {
        // This Class is for writing Errors
        public void WriteError(string Message)
        {
            Console.WriteLine(Message);
            
        }
    }





    public class Number_To_Text_Convertor{
        // This Class convert Numbers to Text
        // NumberWord is the parameters for number word
        public string[] NumberWord { get; set;  }//for extending the numbers by passing parametter
        IErrorHandler MyErrorHandler = new ErrorHandler(); // for handeling errors

        public Number_To_Text_Convertor(string[] _NumberWord = null)
        {
            // Check parameter
            if (_NumberWord is null){
                NumberWord = _NumberWord ?? new string[] { "", " thousand ", " million ", " billion " };
            }else{
                NumberWord = _NumberWord;
            }
        }
        
        public String Convertor(string num)
        {
            //This function is dedicated to converting numerical values into their textual representations.
            //The function operates by segmenting the numbers from right to left in groups of three digits and subsequently employs the Three_Digits_Numbers function for the conversion process.
            //To enhance its capacity, you can add to the NumberWord variable. 
            string[] num_str_array = new string[(num.Length/3)+1];
            char[] num_char = num.ToCharArray();
            string result = "";
            string temporary = "";
            int char_counter;
            int counter = 0;
            int counter2 = 1;
            num_str_array[counter++] = temporary;
            temporary = "";
            try{

                // Checking is the number in correct format
                long NumberChecker = 0;
                if (!long.TryParse(num, out NumberChecker) || Convert.ToInt64(num) < 0)
                {
                    string Error = "Error : Number is not valid.";
                    MyErrorHandler.WriteError(Error);
                    return Error;
                }
                if(Convert.ToInt64(num) == 0){
                    return "Zero";
                }
                // finding left numbers if they are not 3 digits
                for(char_counter = 0; char_counter < num.Length%3; char_counter++){ 
                    temporary += num_char[char_counter];
                }
                num_str_array[0] = temporary;
                temporary = "";
                // seperating each 3 digit 
                for(int j = char_counter; j < num.Length; j++){ 
                    temporary += num_char[j];
                    if(counter2 == 3){
                        num_str_array[counter] = temporary;
                        counter += 1;
                        counter2 = 0;
                        temporary = "";
                    }
                    counter2 += 1;
                }
                counter = 0;
                if(num_char.Length>NumberWord.Length*3){
                    string Error = "Warning : Your number is too long, Please make it shorter or change the NumberWord parameter";
                    MyErrorHandler.WriteError(Error);
                    return Error;
                }
                // converting each 3 digits from right to left
                for(int i = num_str_array.Length-1; i >=0 ; i--){ 
                    if(num_str_array[i] != "" && Convert.ToInt16(num_str_array[i]) != 0){
                        result = Three_Digits_Numbers(num_str_array[i]) + NumberWord[counter] + result ;
                    }
                    counter += 1;
                }
                result = result.Replace("  "," ");
                return result;
            }catch(Exception ex){
                string Error = "Error in Convertor function : " + ex.Message;
                MyErrorHandler.WriteError(Error);
                return Error;
            }
        }

        private String Three_Digits_Numbers(string num){

            //This function is specifically engineered for the conversion of three-digit numbers. 
            //Within this function, the One_nineteen and Two_Digit_Numbers functions are employed to convert two-digit numbers as part of the overall process.    
            char[] num_digitss = num.ToCharArray();
            int Int_Num = Convert.ToInt32(num);
            string first_digit = Convert.ToString(num_digitss[0]);
            string rest_digits = "";
            try
                {
                // converting 2 digit numbers
                if(num_digitss.Length <= 2) 
                {
                    return Two_Digit_Numbers(num);
                }
                // checking 0 in 3 digit numbers
                if(first_digit != "0"){ 
                    rest_digits = One_nineteen(first_digit) + " hundred " + Two_Digit_Numbers(Convert.ToString(num_digitss[1]) + Convert.ToString(num_digitss[2]));
                }else{
                    rest_digits = One_nineteen(first_digit) + Two_Digit_Numbers(Convert.ToString(num_digitss[1]) + Convert.ToString(num_digitss[2]));
                }
                return rest_digits;
            }catch(Exception ex){
                string Error = "Error in Three_Digits_Numbers function : " + ex.Message;
                MyErrorHandler.WriteError(Error);
                return Error;
            }
        }

        private String Two_Digit_Numbers(string num){
            // This function is designed to handle two-digit numbers, including those between 1 and 19 which are processed using the One_nineteen function. 
            // Its primary focus is on numbers falling within the range of 20 to 99, converting them into their respective textual representations.
            
            char[] num_digitss = num.ToCharArray();
            int Int_Num = Convert.ToInt16(num);
            string first_digit = Convert.ToString((num_digitss[0]));
            string second_digit = "";//Convert.ToString((num_characters[1]));
            string result = "";
            try{
                // converting numbers ftom 1 to 19
                if (Int_Num < 20)
                {
                    return One_nineteen(num);
                }
                else
                {
                    string[] Second_digit_words = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    result = Second_digit_words[Convert.ToInt16(first_digit)];

                    // converting numbers from 20 to 99
                    for(int i = 1 ; i < num.Length ; i++){
                        second_digit += Convert.ToString(num_digitss[i]);
                    }
                    // Checking for hyphens
                    if(second_digit != "0"){ 
                        result = result + "-" + One_nineteen(second_digit);
                    }
                    else{ 
                        result = result + " " + One_nineteen(second_digit);
                    }
                    return result;
                }
            }catch(Exception ex){
                string Error = "Error in Two_Digit_Numbers function : " + ex.Message;
                MyErrorHandler.WriteError(Error);
                return Error;
            }
        }

        private string One_nineteen(string num)
        {
            //In this function, we perform the conversion of numbers ranging from 1 to 19.
            //These numbers are associated with distinct, non-repeating words in their representation.
            try{
                string[] OneToNineteen = {"", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                int Int_Num = Convert.ToInt16(num);
                if(Int_Num > 19){
                    return "";
                }
                return OneToNineteen[Int_Num];
                }
            catch(Exception ex){
                string Error = "Error in One_nineteen function : " + ex.Message;
                MyErrorHandler.WriteError(Error);
                return Error;
            }
        }
    
    }
}







namespace NumberToTextConvertorTests
{
    [TestFixture]
    public class ConvertorTests
    {
        [Test]
        public void TestConvertor()
        {
            // Arrange
            Number_To_Text_Convertor convertor = new Number_To_Text_Convertor();

            // Act
            string result = convertor.Convertor("10");

            // Assert
            Assert.AreEqual("ten", result); // Adjust expected output as needed
        }
    }
}
