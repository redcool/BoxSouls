using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSouls
{
    public class WeaponInfo : MonoBehaviour
    {
        public Collider c;
        public CharacterControl owner;
        // Start is called before the first frame update
        void Start()
        {
            c = GetComponentInChildren<Collider>();
            c.isTrigger = true;
            CloseTrigger();
        }

        public void Init(CharacterControl owner)
        {
            this.owner = owner;
        }

        public void OpenTrigger()
        {
            c.enabled = true;
        }

        public void CloseTrigger()
        {
            c.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            if (other.CompareTag(Consts.Tags.CanHit))
            {
                var control = other.GetComponent<CharacterControl>();
                if (control)
                {
                    control.OnDamage(owner.characterStats);
                }
            }
        }

    }
}
