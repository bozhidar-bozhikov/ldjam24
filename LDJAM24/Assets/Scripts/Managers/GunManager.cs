using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class GunManager : MonoBehaviour
{
    public static GunManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public SMG smg;
    public AssaultRifle ar;
    public DoubleBarrelShotgun shotgun;
    public Sniper sniper;
    public GrenadeLauncher grenadeLauncher;
    public Melee melee;
    public Gun currentGun;
    private Gun previousGun;
    public int consecutiveSummons = 0;
    private List<Gun> guns;

    public Image weaponSelection;
    public Image costBar;
    public RectTransform costBarFill;
    public List<Sprite> selections;
    public Sprite[] bars;
    public Transform weaponsContent;
    public Animator contentAnimator;

    public bool isGunHolderActive;

    public int scrollSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        melee.animator.SetBool("Show", true);
        melee.StartCoroutine(melee.Unsheathe());
        //gunHolder.gameObject.SetActive(false);
        guns = new List<Gun> { smg, ar, shotgun, sniper, grenadeLauncher };
        guns[0].gunName = "SMG";
        guns[1].gunName = "Autocannon";
        guns[2].gunName = "Shotgun";
        guns[3].gunName = "Railgun";
        guns[4].gunName = "Grenade Launcher";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isGunHolderActive = true;
            //gunHolder.localPosition = Vector3.zero;

            contentAnimator.SetBool("Show", isGunHolderActive);
            weaponSelection.sprite = selections[2];
            SetBarPosition(2);
            UpdateCostBarFill(2);
            //gunName.text = guns[2].gunName;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isGunHolderActive = false;
            contentAnimator.SetBool("Show", isGunHolderActive);
            SummonWeapon();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            DiscardWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && isGunHolderActive)
        {
            SummonWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGunHolderActive)
        {
            SummonWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isGunHolderActive)
        {
            SummonWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && isGunHolderActive)
        {
            SummonWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && isGunHolderActive)
        {
            SummonWeapon(4);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (isGunHolderActive && scrollInput != 0)
        {
            int currentIndex = selections.IndexOf(weaponSelection.sprite);
            currentIndex = Mathf.RoundToInt(
                Mathf.Clamp(currentIndex + -Mathf.Sign(scrollInput) * scrollSpeed, 0, selections.Count - 1));

            SetBarPosition(currentIndex);

            //if (selections.IndexOf(weaponSelection.sprite) == GetCurrentGunIndex())

            if (guns[currentIndex] == previousGun)
            {
                print("selected current weapon");
                UpdateCostBarFill(currentIndex, true);
            }
            else
            {
                UpdateCostBarFill(currentIndex, false);
            }

            weaponSelection.sprite = selections[currentIndex];

            //gunHolder.localPosition += Vector3.right * Mathf.Sign(scrollInput) * scrollSpeed;
            //gunHolder.localPosition = new Vector3(Mathf.Clamp(gunHolder.localPosition.x, -230, 230), 0, 0);

            //int currentIndex = guns.Count - 1 - Mathf.RoundToInt(gunHolder.localPosition.x + 230) / 115;
            //gunName.text = guns[currentIndex].gunName;
        }
    }

    private void SetBarPosition(int index)
    {
        int yPos = 0;

        switch (index)
        {
            case 0: yPos = 8; break;
            case 1:
            case 2:
            case 3:
                yPos = (index - 1) * -8; break;
            case 4: yPos = -24; break;
        }

        costBar.transform.localPosition = new Vector3(costBar.transform.localPosition.x,
            yPos, costBar.transform.localPosition.z);
    }

    void SummonWeapon()
    {
        if (currentGun != null) return;

        int gunIndex = selections.IndexOf(weaponSelection.sprite);

        SummonWeapon(gunIndex);
    }

    void SummonWeapon(int index)
    {
        print(selections.IndexOf(weaponSelection.sprite) + " " + index);
        bool summonedConsecutiveWeapon = selections.IndexOf(weaponSelection.sprite) == index;

        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].enabled = index == i;

            if (index == i)
            {
                currentGun = guns[i];
            }

        }

        print("equipped " + currentGun.GetType().Name);
        PlayerStats.TakeDamage(GetGunCost(index) * 2 + consecutiveSummons);

        consecutiveSummons = summonedConsecutiveWeapon == true ? consecutiveSummons + 1 : 0;

        previousGun = currentGun;
        currentGun.bullets = currentGun.maxBullets;

        melee.StartCoroutine(melee.Sheathe());

        weaponSelection.sprite = selections[2];
        SetBarPosition(2);
        UpdateCostBarFill(2);

        if (isGunHolderActive) contentAnimator.SetBool("Show", false);
        isGunHolderActive = false;

        currentGun.Summon();
    }

    void DiscardWeapon()
    {
        melee.StartCoroutine(melee.Unsheathe());

        if (currentGun == null) return;

        currentGun.Discard();
        previousGun = currentGun;
        currentGun.enabled = false;
        currentGun = null;
    }

    public static void RanOut()
    {
        instance.DiscardWeapon();
    }

    public int GetGunCost(int index)
    {
        int cost = 0;

        switch (index)
        {
            case 0: cost = 2; break;
            case 1: cost = 2; break;
            case 2: cost = 3; break;
            case 3: cost = 3; break;
            case 4: cost = 4; break;
        }

        return cost;
    }

    public int GetCurrentGunIndex()
    {
        if (currentGun == null) return -1;

        switch (currentGun.gunName)
        {
            case "SMG": return 0;
            case "Autocannon": return 1;
            case "Shotgun": return 2;
            case "Railgun": return 3;
            case "Grenade Launcher": return 4;
            default:
                return -1;
        }
    }

    private void UpdateCostBarFill(int index)
    {
        costBarFill.sizeDelta = new Vector2(64 - (GetGunCost(index) + consecutiveSummons) * 4, 8);
    }

    private void UpdateCostBarFill(int index, bool addConsecutiveSummons)
    {
        if (addConsecutiveSummons == false)
            costBarFill.sizeDelta = new Vector2(64 - (GetGunCost(index)) * 4, 8);
        else
            UpdateCostBarFill(index);
    }
}
