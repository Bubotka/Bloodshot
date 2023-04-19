using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public DashSkill Dash { get; private set; }

    public CloneSkill Clone { get; private set; }

    public SwordSkill Sword { get; private set; }

    public BlackholeSkill Blackhole { get; private set; }

    public CrystalSkill Crystal { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
        Sword = GetComponent<SwordSkill>();
        Blackhole = GetComponent<BlackholeSkill>();
        Crystal = GetComponent<CrystalSkill>();
    }
}
