﻿using UnityEngine;
using System.Collections;
using UMA;

public class UMACharGen : MonoBehaviour
{

    public UMAGeneratorBase generator;
    public SlotLibrary slotLibrary;
    public OverlayLibrary overlayLibrary;
    public RaceLibrary raceLibrary;
    public RuntimeAnimatorController animationController;
	public RuntimeAnimatorController secondaryAnimationController;

    private UMADynamicAvatar umaDynamicAvatar;
    private UMAData umaData;
    private UMADnaHumanoid umaDna;
    private UMADnaTutorial umaTutorialDNA;

    private int numberOfSlots = 10;

	public bool isMultiSpawn = false;

	private GameObject m_createdObject = null;
	private int m_spawnCount = 0;

    void Start()
    {
		setupFallbackObjects ();
		m_createdObject = GenerateUMA(name + "( generated object " + m_spawnCount + ")");
		++m_spawnCount;
		if (!isMultiSpawn) {
			Destroy (gameObject);
		}
    }

	void Update()
	{
		if (m_createdObject.GetComponent<HealthComponent> ().isDead ()) {
			m_createdObject = GenerateUMA (name + "( generated object " + m_spawnCount + ")");
			++m_spawnCount;
		}
	}

	void setupFallbackObjects()
	{
		if (generator == null) {
			generator = GameObject.Find("UMAGenerator").GetComponent<UMAGenerator>();
		}
		if (slotLibrary == null) {
			slotLibrary = GameObject.Find("SlotLibrary").GetComponent<SlotLibrary>();
		}
		if (overlayLibrary == null) {
			overlayLibrary = GameObject.Find("OverlayLibrary").GetComponent<OverlayLibrary>();
		}
		if (raceLibrary == null) {
			raceLibrary = GameObject.Find("RaceLibrary").GetComponent<RaceLibrary>();
		}
	}

    GameObject GenerateUMA(string name)
    {
		// Create a new game object and add UMA components to it
		GameObject GO = new GameObject(name);
		umaDynamicAvatar = GO.AddComponent<UMADynamicAvatar>();
		umaDynamicAvatar.animationController = animationController;
		GO.AddComponent<RagdollCreatorTest>();
		if (name.Contains ("Zombie")) {
			GO.AddComponent<ZombieBehavior> ();
			GO.tag = "Zombie";
		} else {
			HumanBehavior hb = GO.AddComponent<HumanBehavior> ();
			hb.zombieAnimationController = secondaryAnimationController;
			GO.tag = "Human";
		}
		GO.AddComponent<NavMeshAgent> ();
		GO.AddComponent<HealthComponent> ();
		GO.transform.position = transform.position;

        // Initialize Avatar and grab a reference to its data component
        umaDynamicAvatar.Initialize();
        umaData = umaDynamicAvatar.umaData;

        // Attach our generator
        umaDynamicAvatar.umaGenerator = generator;
        umaData.umaGenerator = generator;

        // Set up slot Array
        umaData.umaRecipe.slotDataList = new SlotData[numberOfSlots];

        // Set up our Morph references
        umaDna = new UMADnaHumanoid();
        umaTutorialDNA = new UMADnaTutorial();
        umaData.umaRecipe.AddDna(umaDna);
        umaData.umaRecipe.AddDna(umaTutorialDNA);


		if (name.Contains ("Female")) {
			CreateFemale ();
		} else {
			CreateMale ();
		}

        // Generate our UMA
        umaDynamicAvatar.UpdateNewRace();
		
		return GO;
    }

    void CreateFemale()
    {
        // Grab a reference to our recipe
        var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
        umaRecipe.SetRace(raceLibrary.GetRace("HumanFemale"));

        umaData.umaRecipe.slotDataList[0] = slotLibrary.InstantiateSlot("FemaleHead_Head");
        umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleHead01"));

        umaData.umaRecipe.slotDataList[1] = slotLibrary.InstantiateSlot("FemaleTorso");
        umaData.umaRecipe.slotDataList[1].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleBody01"));

        umaData.umaRecipe.slotDataList[2] = slotLibrary.InstantiateSlot("FemaleTshirt01");
        umaData.umaRecipe.slotDataList[2].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleShirt01"));

        umaData.umaRecipe.slotDataList[3] = slotLibrary.InstantiateSlot("FemaleFace");
        umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleHead01"));

        umaData.umaRecipe.slotDataList[4] = slotLibrary.InstantiateSlot("FemaleLongHair01");
        umaData.umaRecipe.slotDataList[4].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleLongHair01_Module"));

        umaData.umaRecipe.slotDataList[5] = slotLibrary.InstantiateSlot("FemaleHands");
        umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleBody01"));

        umaData.umaRecipe.slotDataList[6] = slotLibrary.InstantiateSlot("FemaleLegs");
        umaData.umaRecipe.slotDataList[6].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleJeans01"));

        umaData.umaRecipe.slotDataList[7] = slotLibrary.InstantiateSlot("FemaleFeet");
        umaData.umaRecipe.slotDataList[7].AddOverlay(overlayLibrary.InstantiateOverlay("FemaleBody01"));
    }

	void CreateMale()
	{
		// Grab a reference to our recipe
		var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
		umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));

		umaData.umaRecipe.slotDataList[0] = slotLibrary.InstantiateSlot("MaleHead_Head");
		umaData.umaRecipe.slotDataList[0].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHead01"));

		umaData.umaRecipe.slotDataList[1] = slotLibrary.InstantiateSlot("MaleTorso");
		umaData.umaRecipe.slotDataList[1].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));

		umaData.umaRecipe.slotDataList[3] = slotLibrary.InstantiateSlot("MaleFace");
		umaData.umaRecipe.slotDataList[3].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHead01"));

		umaData.umaRecipe.slotDataList[4] = slotLibrary.InstantiateSlot("MaleHead_Head");
		umaData.umaRecipe.slotDataList[4].AddOverlay(overlayLibrary.InstantiateOverlay("MaleHair01"));

		umaData.umaRecipe.slotDataList[5] = slotLibrary.InstantiateSlot("MaleHands");
		umaData.umaRecipe.slotDataList[5].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));

		umaData.umaRecipe.slotDataList[6] = slotLibrary.InstantiateSlot("MaleLegs");
		umaData.umaRecipe.slotDataList[6].AddOverlay(overlayLibrary.InstantiateOverlay("MaleJeans01"));

		umaData.umaRecipe.slotDataList[7] = slotLibrary.InstantiateSlot("MaleFeet");
		umaData.umaRecipe.slotDataList[7].AddOverlay(overlayLibrary.InstantiateOverlay("MaleBody01"));
	}
}
