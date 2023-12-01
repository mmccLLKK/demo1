using System;
using UnityEngine;

/// <summary>
/// AI
/// </summary>
public class RoleAI : MonoBehaviour
{

    public Role role;
    
    public void Awake()
    {
        role = GetComponent<Role>();
    }

    private void Update()
    {
        
    }
}