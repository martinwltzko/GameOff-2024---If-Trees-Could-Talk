using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableRelay : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private UnityEvent _onFocus;
        [SerializeField] private UnityEvent _onRelease;
    
        public Transform Transform => transform;
        public void Interact(object sender)
        {
            _onInteract.Invoke();
        }

        public void Focus(object sender)
        {
            _onFocus.Invoke();
        }

        public void Release(object sender)
        {
            _onRelease.Invoke();
        }
    }
}
