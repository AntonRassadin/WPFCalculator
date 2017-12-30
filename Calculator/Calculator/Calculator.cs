using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    enum OperationName { Equals, Divide, Multiply, Substract, Add};

    class Operation
    {
        public delegate double OperationMethod(double first, double second);
        private OperationMethod methodName;
        private string operationSign;

        internal OperationMethod MethodName { get => methodName; set => methodName = value; }
        public string OperationSign { get => operationSign; set => operationSign = value; }

        public Operation(OperationMethod methodName, string operationSign)
        {
            this.MethodName = methodName;
            this.OperationSign = operationSign;
        }
    }

    class Calc
    {
        private string outputResult= "0";
        private string currentNumber = "0";
        private string currentState;
        private double firstNumber;
        private double secondNumber;
        private bool firstNumberExist;
        private bool currentOperationIsNotOver;
        private bool divideByZeroException;
        private OperationName currentOperation = OperationName.Equals;

        public string OutputResult { get => outputResult; set { outputResult = value; currentNumber = outputResult; } }

        public string CurrentState { get => currentState; set => currentState = value; }

        private Dictionary<OperationName, Operation> operationList = new Dictionary<OperationName, Operation>();
        
        public Calc()
        {
            operationList.Add(OperationName.Equals, null);
            operationList.Add(OperationName.Add, new Operation(Add, "+"));
            operationList.Add(OperationName.Substract, new Operation(Substract, "-"));
            operationList.Add(OperationName.Multiply, new Operation(Multiply, "*"));
            operationList.Add(OperationName.Divide, new Operation(Divide, "/"));
        }

        public void PerformOperation(string parameter)
        {
            switch (parameter)
            {
                case "/":
                    CheckAndPerform(OperationName.Divide);
                    break;
                case "=":
                    CheckAndPerform(OperationName.Equals);
                    break;
                case "*":
                    CheckAndPerform(OperationName.Multiply);
                    break;
                case "-":
                    CheckAndPerform(OperationName.Substract);
                    break;
                case "+":
                    CheckAndPerform(OperationName.Add);
                    break;
                case "C":
                    ResetAll();
                    break;
                default:
                    if (currentNumber == "0")
                    {
                        OutputResult = parameter;
                        break;
                    }
                    if (OutputResult.Length > 15)
                    {
                        break;
                    }
                    OutputResult += parameter;
                    break;
            }
 
        }

        private void ResetAll()
        {
            currentOperation = OperationName.Equals;
            firstNumber = 0;
            firstNumberExist = false;
            currentOperationIsNotOver = false;
            secondNumber = 0;
            OutputResult = "0";
            currentState = "";
            divideByZeroException = false;
        }

        private double Add(double first, double second)
        {
            return (first + second);
        }

        private double Substract(double first, double second)
        {
            return (first - second);
        }

        private double Multiply(double first, double second)
        {
            return (first * second);
        }

        private double Divide(double first, double second)
        {
            if(second == 0)
            {
                divideByZeroException = true;
                return 0;
            }
            return (first / second);
        }

        private delegate double OperationDelegate(double firstNumber, double secondNumber);

        private void CheckAndPerform(OperationName selectedOperation)
        {
            if (divideByZeroException)
            {
                return;
            }

            if (currentOperationIsNotOver)
            {
                currentOperationIsNotOver = false;
                CheckAndPerform(currentOperation);
                if (divideByZeroException)
                {
                    return;
                }
            }

            if (selectedOperation == OperationName.Equals)
            {
                currentOperation = OperationName.Equals;
                return;
            }

            currentOperation = selectedOperation;
            if (!firstNumberExist)
            {
                firstNumber = Convert.ToDouble(outputResult);
                currentState = outputResult + " " + operationList[currentOperation].OperationSign;
                OutputResult = "0";
                firstNumberExist = true;
                currentOperationIsNotOver = true;
            }
            else
            {
                secondNumber = Convert.ToDouble(outputResult);
                OutputResult = operationList[currentOperation].MethodName(firstNumber, secondNumber).ToString();
               
                currentState = "";
                currentNumber = "0";
                firstNumber = 0;
                firstNumberExist = false;
                if (divideByZeroException)
                {
                    currentState = "Divide by Zero is";
                    outputResult = "IMPOSIBURU!!!";
                }
            }
        }
    }
}
