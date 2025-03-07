using UnityEngine;
using UnityEngine.UI;

namespace UnitCreation
{
    public class PartTumblerController : MonoBehaviour
    {
        private static readonly int IsTurnedOn = Animator.StringToHash("isTurnedOn");
        private static readonly int ValueChanged = Animator.StringToHash("ValueChanged");
        
        private bool _isTurnedOn = true;
        private Animator _animator;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _isTurnedOn = !_isTurnedOn;
            _animator.SetTrigger(ValueChanged);
            _animator.SetBool(IsTurnedOn, _isTurnedOn);
        }

        public bool GetState()
        {
            return _isTurnedOn;
        }
        
    }
}
