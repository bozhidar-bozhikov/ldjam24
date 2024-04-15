using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public int consecutiveSummons = 0;
    private List<Gun> guns;

    public Image weaponSelection;
    public Image costBar;
    public List<Sprite> selections;
    public Sprite[] bars;
    public Transform weaponsContent;
    public Animator contentAnimator;

    public bool isGunHolderActive;

    public int scrollSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        melee.enabled = true;
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
            //gunName.text = "";
            //gunHolder.gameObject.SetActive(false);
            SummonWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGunHolderActive)
        {
            //gunName.text = "";
            //gunHolder.gameObject.SetActive(false);
            SummonWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isGunHolderActive)
        {
            //gunName.text = "";
            //gunHolder.gameObject.SetActive(false);
            SummonWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && isGunHolderActive)
        {
            //gunName.text = "";
            //gunHolder.gameObject.SetActive(false);
            SummonWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && isGunHolderActive)
        {
            //gunName.text = "";
            //gunHolder.gameObject.SetActive(false);
            SummonWeapon(4);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (isGunHolderActive && scrollInput != 0)
        {
            int currentIndex = selections.IndexOf(weaponSelection.sprite);
            currentIndex = Mathf.RoundToInt(
                Mathf.Clamp(currentIndex + -Mathf.Sign(scrollInput) * scrollSpeed, 0, selections.Count - 1));

            weaponSelection.sprite = selections[currentIndex];
            SetBarPosition(currentIndex);

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
        for (int i = 0; i < guns.Count; i++)
        {
            guns[i].enabled = index == i;

            if (index == i)
            {
                if (currentGun == guns[i]) consecutiveSummons++;
                else consecutiveSummons = 0;

                currentGun = guns[i];
            }

        }

        print("equipped " + currentGun.GetType().Name);

        weaponSelection.sprite = selections[2];
        SetBarPosition(2);
        currentGun.bullets = currentGun.maxBullets;
        melee.enabled = false;

        if (isGunHolderActive) contentAnimator.SetBool("Show", false);
        isGunHolderActive = false;
    }

    void DiscardWeapon()
    {
        if (currentGun == null)
        {
            melee.enabled = true;
            return;
        }

        currentGun.enabled = false;
        currentGun = null;
        consecutiveSummons = 0;
        melee.enabled = true;
        print("equipped melee");
    }

    public static void RanOut()
    {
        instance.DiscardWeapon();
    }
}
