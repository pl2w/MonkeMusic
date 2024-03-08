using System;
using UnityEngine;

namespace MonkeMusic.Behaviours.Buttons
{
    public class PressableButton : MonoBehaviour
    {
        public Action OnPress;
        float touchTime;
        float debounceTime = 0.2f;

        public PressableButton() => gameObject.layer = 18;

        public void OnTriggerEnter(Collider collider)
        {
            if (!enabled || !(touchTime + debounceTime < Time.time) || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() == null)
            {
                return;
            }

            touchTime = Time.time;
            OnPress.Invoke();
            GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (!(component == null))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
            }
        }
    }
}
