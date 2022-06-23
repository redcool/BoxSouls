using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BoxSouls
{
    [Serializable]
    public class CharacterStats
    {
        [Header("Health")]
        public int maxHp = 100;
        public int hp =100;

        [Header("Energy")]
        public float maxEnergy = 100;
        public float energy = 100;
        public float energyResume = 10;
        public float energyResumeInterval = 3;

        [Header("Attack Defence")]
        public int attack = 3;
        public int defence = 3;
        public int impale = 1;

        public float HpRate => (float)hp / maxHp;
        public float EnergyRate => (float)energy / maxEnergy;

        public void ConsumeEnergy(int v)
        {
            energy -= v;
            energy = Mathf.Max(0, energy);
        }

        public void Resume()
        {
            if(Mathf.Approximately(energy,maxEnergy))
            {
                energy = maxEnergy;
                return;
            }

            energy += (energyResume * Time.deltaTime);
            energy = Mathf.Min(energy, maxEnergy);
        }
    }
}
