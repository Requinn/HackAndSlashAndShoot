using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIEntity : Entity{
    public Weapon weapon;
    protected NavMeshAgent _NMAgent;
    
    /// <summary>
    /// this is for fsm experiments
    /// </summary>
    public void Move() {
        Movement();
    }

    public abstract void ProjectileResponse();

}
