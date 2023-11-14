using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Sword Skill Info")]
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDirection;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        SpawnDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Vector2 aimDirection = AimDirection();
            finalDirection = new Vector2(aimDirection.normalized.x * launchForce.x, aimDirection.normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.X))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = ObjectPoolManager.instance.SpawnFromPool("Sword", player.transform.position, player.transform.rotation);
        newSword.transform.parent = null;
        SwordSkillController swordSkillController = newSword.GetComponent<SwordSkillController>();

        switch (swordType)
        {
            case SwordType.Bounce:
                swordSkillController.SetupBounce(true, bounceAmount);
                break;
            case SwordType.Pierce:
                swordSkillController.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                swordSkillController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
                break;
        }

        player.sword = newSword;

        swordSkillController.SetupSword(finalDirection, swordGravity, player);

        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 positionToThrow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = positionToThrow - playerPosition;

        return direction;
    }

    private void SpawnDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = ObjectPoolManager.instance.SpawnFromPool("AimDot", player.transform.position, Quaternion.identity);
            dots[i].transform.SetParent(dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 aimDirection = AimDirection();
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            aimDirection.normalized.x * launchForce.x,
            aimDirection.normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
