using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;

	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;

	BuildManager buildManager;
	[HideInInspector]
	private GameObject[] tur;
	[HideInInspector]
	private GameObject[] tur_upgrade;

	void Awake() {
		tur = new GameObject[3];
		tur_upgrade = new GameObject[3];

		tur[0] = Resources.Load<GameObject>("StandardTurret") as GameObject;
        tur[1] = Resources.Load<GameObject>("MissileLauncher") as GameObject;
        tur[2] = Resources.Load<GameObject>("LaserBeamer") as GameObject;

		tur_upgrade[0] = Resources.Load<GameObject>("StandardTurret_Upgraded") as GameObject;
		tur_upgrade[1] = Resources.Load<GameObject>("MissileLauncher_Upgraded") as GameObject;
		tur_upgrade[2] = Resources.Load<GameObject>("LaserBeamer_Upgraded") as GameObject;
	}
	void Start ()
	{
		buildManager = BuildManager.instance;
		turretBlueprint = new TurretBlueprint(100);
    }

	public Vector3 GetBuildPosition ()
	{
		return transform.position + positionOffset;
	}

	void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (turret != null)
		{
			buildManager.SelectNode(this);
			return;
		}

            buildManager.SelectBuild(this);
            return;
	}

	public void BuildTurret ()
	{
		if (PlayerStats.Money < turretBlueprint.cost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

		PlayerStats.Money -= turretBlueprint.cost;

		GameObject _turret = (GameObject)Instantiate(tur[Random.Range(0,3)], GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Debug.Log("Turret build!");
	}

	public void UpgradeTurret ()
	{

		if (PlayerStats.Money < turretBlueprint.cost)
		{
			Debug.Log("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= turretBlueprint.cost;

        Destroy(turret);

		//Build a new one
		GameObject _turret = (GameObject)Instantiate(tur_upgrade[Random.Range(0,3)], GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		isUpgraded = true;
		turretBlueprint.upgradeTurret();

		Debug.Log("Turret upgraded!");
	}

	public void SellTurret ()
	{
		PlayerStats.Money += turretBlueprint.GetSellAmount();

		GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(turret);
		turretBlueprint.init();
		turret = null;
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
			return;
	}
}
