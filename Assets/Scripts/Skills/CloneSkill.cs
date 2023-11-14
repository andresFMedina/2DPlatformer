using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]    
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;


    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = ObjectPoolManager.instance.SpawnFromPool("Clone", Vector3.zero, Quaternion.identity);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack);
    }
}
