using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Menu
{
    public enum ValidatorState
    {
        Correct,
        ProblemWithCode,
        ProblemWithConstruction,
    }
    public class Validator : MonoBehaviour
    {
        public List<Coroutine> executions = new List<Coroutine>();

        public SyntaxHighlighter codeText;
        

        private bool IsValidCode()
        {
            string code = codeText.GetWroteCodeText();
            if (!code.Contains("while probe.run():"))
            {
                return false;
            }
            return true;
        }

        private bool IsValidConstruction()
        {
            return true;
        }

        public void StopAllExecutions()
        {
            foreach (Coroutine coroutine in executions)
            {
                StopCoroutine(coroutine);
            }
            executions.Clear();
        }

        public ValidatorState Validate()
        {
            if (!IsValidConstruction())
            {
                return ValidatorState.ProblemWithConstruction;
            }

            if (!IsValidCode())
            {
                return ValidatorState.ProblemWithCode;
            }
            return ValidatorState.Correct;
        }
        
    }
}
