﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
			ZombieBehavior zbh = GO.AddComponent<ZombieBehavior> ();
			zbh.speedMultiplier = Random.Range (0.5f, 2.5f);
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
		setupDna (umaDna);
        umaDynamicAvatar.UpdateNewRace();
		
		return GO;
    }

	private SlotLibrary GetSlotLibrary()
	{
		return slotLibrary;
	}

	private OverlayLibrary GetOverlayLibrary()
	{
		return overlayLibrary;
	}

    void CreateFemale()
    {
        var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
        umaRecipe.SetRace(raceLibrary.GetRace("HumanFemale"));

		// copied from Bill's code

		Color skinColor = new Color(1, 1, 1, 1);
		float skinTone;

		skinTone = Random.Range(0.1f, 0.6f);
		skinColor = new Color(skinTone + Random.Range(0.35f, 0.4f), skinTone + Random.Range(0.25f, 0.4f), skinTone + Random.Range(0.35f, 0.4f), 1);

		Color HairColor = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1);

		int randomResult = 0;
		//Female Avatar

		//Example of dynamic list
		List<SlotData> tempSlotList = new List<SlotData>();


		//tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleEyes"));
		//tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("EyeOverlay"));
		//tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("EyeOverlayAdjust", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleTorso"));
		int headIndex = 0;

		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{

			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleFace"));
			headIndex = tempSlotList.Count - 1;
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleHead01", skinColor));
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleEyebrow01", new Color(0.125f, 0.065f, 0.065f, 1.0f)));

			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleLipstick01", new Color(skinColor.r + Random.Range(0.0f, 0.3f), skinColor.g, skinColor.b + Random.Range(0.0f, 0.2f), 1)));
			}
		}
		else if (randomResult == 1)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Head"));
			headIndex = tempSlotList.Count - 1;
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleHead01", skinColor));
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleEyebrow01", new Color(0.125f, 0.065f, 0.065f, 1.0f)));

			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Eyes", tempSlotList[headIndex].GetOverlayList()));
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Mouth", tempSlotList[headIndex].GetOverlayList()));
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Nose", tempSlotList[headIndex].GetOverlayList()));


			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Ears", tempSlotList[headIndex].GetOverlayList()));

				//   tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_ElvenEars"));
				//tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("ElvenEars", skinColor));
			}
			else if (randomResult == 1)
			{
				tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHead_Ears", tempSlotList[headIndex].GetOverlayList()));
			}

			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleLipstick01", new Color(skinColor.r + Random.Range(0.0f, 0.3f), skinColor.g, skinColor.b + Random.Range(0.0f, 0.2f), 1)));
			}
		}

		tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleEyelash"));
		tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleEyelash", Color.black));



		tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleTorso"));

		if (this.gameObject.tag == "SpawnPoint_Zombie")
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("ZombieGirl_BodyNew"));
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("ZombieGirl_TopNew"));
			//tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("ZombieGirlTop", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));

			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("ZombieGirl_PantsNew"));
		}


		int bodyIndex = tempSlotList.Count - 1;
		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleBody01", skinColor));
		}
		if (randomResult == 1)
		{
			tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleBody02", skinColor));
		}



		tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleUnderwear01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));

		randomResult = Random.Range(0, 4);
		if (randomResult == 0)
		{
			tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleShirt01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}
		else if (randomResult == 1)
		{

			tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleShirt02", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));

		}
		else if (randomResult == 2)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleTshirt01"));
			tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleTshirt01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}
		else
		{

		}




		tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleHands", tempSlotList[bodyIndex].GetOverlayList()));

		//tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleInnerMouth"));
		//tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("InnerMouth"));

		tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleFeet", tempSlotList[bodyIndex].GetOverlayList()));


		randomResult = Random.Range(0, 2);

		if (randomResult == 0)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleLegs", tempSlotList[bodyIndex].GetOverlayList()));
		}
		else if (randomResult == 1)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleLegs", tempSlotList[bodyIndex].GetOverlayList()));
			tempSlotList[bodyIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleJeans01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}

		randomResult = Random.Range(0, 3);
		if (randomResult == 0)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleShortHair01", tempSlotList[headIndex].GetOverlayList()));
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleShortHair01", HairColor));
		}
		else if (randomResult == 1)
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleLongHair01", tempSlotList[headIndex].GetOverlayList()));
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleLongHair01", HairColor));
		}
		else
		{
			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleLongHair01", tempSlotList[headIndex].GetOverlayList()));
			tempSlotList[headIndex].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleLongHair01", HairColor));

			tempSlotList.Add(GetSlotLibrary().InstantiateSlot("FemaleLongHair01_Module"));
			tempSlotList[tempSlotList.Count - 1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("FemaleLongHair01_Module", HairColor));
		}

		umaData.SetSlots(tempSlotList.ToArray());
    }

	void CreateMale()
	{
		var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
		umaRecipe.SetRace(raceLibrary.GetRace("HumanMale"));


		// Copied from Bill's code
		int randomResult = 0;
		//Male Avatar

		Color skinColor = new Color(1, 1, 1, 1);
		float skinTone;

		skinTone = Random.Range(0.1f, 0.6f);
		skinColor = new Color(skinTone + Random.Range(0.35f, 0.4f), skinTone + Random.Range(0.25f, 0.4f), skinTone + Random.Range(0.35f, 0.4f), 1);

		Color HairColor = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1);

		umaData.umaRecipe.slotDataList = new SlotData[15];

		umaData.umaRecipe.slotDataList[0] = GetSlotLibrary().InstantiateSlot("MaleEyes");
		//umaData.umaRecipe.slotDataList[0].AddOverlay(GetOverlayLibrary().InstantiateOverlay("EyeOverlay"));
		//umaData.umaRecipe.slotDataList[0].AddOverlay(GetOverlayLibrary().InstantiateOverlay("EyeOverlayAdjust", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));

		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[1] = GetSlotLibrary().InstantiateSlot("MaleFace");

			randomResult = Random.Range(0, 2);

			if (randomResult == 0)
			{
				umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHead01", skinColor));
			}
			else if (randomResult == 1)
			{
				umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHead02", skinColor));
			}
		}
		else if (randomResult == 1)
		{
			umaData.umaRecipe.slotDataList[1] = GetSlotLibrary().InstantiateSlot("MaleHead_Head");

			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHead01", skinColor));
			}
			else if (randomResult == 1)
			{
				umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHead02", skinColor));
			}

			umaData.umaRecipe.slotDataList[7] = GetSlotLibrary().InstantiateSlot("MaleHead_Eyes", umaData.umaRecipe.slotDataList[1].GetOverlayList());
			umaData.umaRecipe.slotDataList[9] = GetSlotLibrary().InstantiateSlot("MaleHead_Mouth", umaData.umaRecipe.slotDataList[1].GetOverlayList());

			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				//umaData.umaRecipe.slotDataList[10] = GetSlotLibrary().InstantiateSlot("MaleHead_PigNose", umaData.umaRecipe.slotDataList[1].GetOverlayList());
				//umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHead_PigNose", skinColor));
				umaData.umaRecipe.slotDataList[10] = GetSlotLibrary().InstantiateSlot("MaleHead_Nose", umaData.umaRecipe.slotDataList[1].GetOverlayList());

			}
			else if (randomResult == 1)
			{
				umaData.umaRecipe.slotDataList[10] = GetSlotLibrary().InstantiateSlot("MaleHead_Nose", umaData.umaRecipe.slotDataList[1].GetOverlayList());
			}

			randomResult = Random.Range(0, 2);
			if (randomResult == 0)
			{
				umaData.umaRecipe.slotDataList[8] = GetSlotLibrary().InstantiateSlot("MaleHead_Ears", umaData.umaRecipe.slotDataList[1].GetOverlayList());
				// umaData.umaRecipe.slotDataList[8] = GetSlotLibrary().InstantiateSlot("MaleHead_ElvenEars");
				//	umaData.umaRecipe.slotDataList[8].AddOverlay(GetOverlayLibrary().InstantiateOverlay("ElvenEars", skinColor));
			}
			else if (randomResult == 1)
			{
				umaData.umaRecipe.slotDataList[8] = GetSlotLibrary().InstantiateSlot("MaleHead_Ears", umaData.umaRecipe.slotDataList[1].GetOverlayList());
			}
		}


		randomResult = Random.Range(0, 3);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHair01", HairColor * 0.25f));
		}
		else if (randomResult == 1)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleHair02", HairColor * 0.25f));
		}
		else
		{

		}


		randomResult = Random.Range(0, 4);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard01", HairColor * 0.15f));
		}
		else if (randomResult == 1)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard02", HairColor * 0.15f));
		}
		else if (randomResult == 2)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard03", HairColor * 0.15f));
		}
		else
		{

		}



		//Extra beard composition
		randomResult = Random.Range(0, 4);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard01", HairColor * 0.15f));
		}
		else if (randomResult == 1)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard02", HairColor * 0.15f));
		}
		else if (randomResult == 2)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBeard03", HairColor * 0.15f));
		}
		else
		{

		}

		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleEyebrow01", HairColor * 0.05f));
		}
		else
		{
			umaData.umaRecipe.slotDataList[1].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleEyebrow02", HairColor * 0.05f));
		}

		umaData.umaRecipe.slotDataList[2] = GetSlotLibrary().InstantiateSlot("MaleTorso");

		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[2].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBody01", skinColor));
		}
		else
		{
			umaData.umaRecipe.slotDataList[2].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleBody02", skinColor));
		}


		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[2].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleShirt01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}

		umaData.umaRecipe.slotDataList[3] = GetSlotLibrary().InstantiateSlot("MaleHands", umaData.umaRecipe.slotDataList[2].GetOverlayList());

		//umaData.umaRecipe.slotDataList[4] = GetSlotLibrary().InstantiateSlot("MaleInnerMouth");
		//umaData.umaRecipe.slotDataList[4].AddOverlay(GetOverlayLibrary().InstantiateOverlay("InnerMouth"));


		randomResult = Random.Range(0, 2);
		if (randomResult == 0)
		{
			umaData.umaRecipe.slotDataList[4] = GetSlotLibrary().InstantiateSlot("MaleLegs", umaData.umaRecipe.slotDataList[2].GetOverlayList());
			umaData.umaRecipe.slotDataList[4].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleUnderwear01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}
		else
		{
			umaData.umaRecipe.slotDataList[4] = GetSlotLibrary().InstantiateSlot("MaleJeans01");
			umaData.umaRecipe.slotDataList[4].AddOverlay(GetOverlayLibrary().InstantiateOverlay("MaleJeans01", new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1)));
		}

		umaData.umaRecipe.slotDataList[5] = GetSlotLibrary().InstantiateSlot("MaleFeet", umaData.umaRecipe.slotDataList[2].GetOverlayList());
	}



	// copied from Bill's code
	private void setupDna (UMADnaHumanoid umaDna)
	{		
		umaDna.height = Random.Range (0.3f, 0.5f);
		umaDna.headSize = Random.Range (0.485f, 0.515f);
		umaDna.headWidth = Random.Range (0.4f, 0.6f);

		umaDna.neckThickness = Random.Range (0.495f, 0.51f);

		if (umaData.umaRecipe.raceData.raceName == "HumanMale") {
			umaDna.handsSize = Random.Range (0.485f, 0.515f);
			umaDna.feetSize = Random.Range (0.485f, 0.515f);
			umaDna.legSeparation = Random.Range (0.4f, 0.6f);
			umaDna.waist = 0.5f;
		} else {
			umaDna.handsSize = Random.Range (0.485f, 0.515f);
			umaDna.feetSize = Random.Range (0.485f, 0.515f);
			umaDna.legSeparation = Random.Range (0.485f, 0.515f);
			umaDna.waist = Random.Range (0.3f, 0.8f);
		}

		umaDna.armLength = Random.Range (0.485f, 0.515f);
		umaDna.forearmLength = Random.Range (0.485f, 0.515f);
		umaDna.armWidth = Random.Range (0.3f, 0.8f);
		umaDna.forearmWidth = Random.Range (0.3f, 0.8f);

		umaDna.upperMuscle = Random.Range (0.0f, 1.0f);
		umaDna.upperWeight = Random.Range (-0.2f, 0.2f) + umaDna.upperMuscle;
		if (umaDna.upperWeight > 1.0) {
			umaDna.upperWeight = 1.0f;
		}
		if (umaDna.upperWeight < 0.0) {
			umaDna.upperWeight = 0.0f;
		}

		umaDna.lowerMuscle = Random.Range (-0.2f, 0.2f) + umaDna.upperMuscle;
		if (umaDna.lowerMuscle > 1.0) {
			umaDna.lowerMuscle = 1.0f;
		}
		if (umaDna.lowerMuscle < 0.0) {
			umaDna.lowerMuscle = 0.0f;
		}

		umaDna.lowerWeight = Random.Range (-0.1f, 0.1f) + umaDna.upperWeight;
		if (umaDna.lowerWeight > 1.0) {
			umaDna.lowerWeight = 1.0f;
		}
		if (umaDna.lowerWeight < 0.0) {
			umaDna.lowerWeight = 0.0f;
		}

		umaDna.belly = umaDna.upperWeight;
		umaDna.legsSize = Random.Range (0.4f, 0.6f);
		umaDna.gluteusSize = Random.Range (0.4f, 0.6f);

		umaDna.earsSize = Random.Range (0.3f, 0.8f);
		umaDna.earsPosition = Random.Range (0.3f, 0.8f);
		umaDna.earsRotation = Random.Range (0.3f, 0.8f);

		umaDna.noseSize = Random.Range (0.3f, 0.8f);

		umaDna.noseCurve = Random.Range (0.3f, 0.8f);
		umaDna.noseWidth = Random.Range (0.3f, 0.8f);
		umaDna.noseInclination = Random.Range (0.3f, 0.8f);
		umaDna.nosePosition = Random.Range (0.3f, 0.8f);
		umaDna.nosePronounced = Random.Range (0.3f, 0.8f);
		umaDna.noseFlatten = Random.Range (0.3f, 0.8f);

		umaDna.chinSize = Random.Range (0.3f, 0.8f);
		umaDna.chinPronounced = Random.Range (0.3f, 0.8f);
		umaDna.chinPosition = Random.Range (0.3f, 0.8f);

		umaDna.mandibleSize = Random.Range (0.45f, 0.52f);
		umaDna.jawsSize = Random.Range (0.3f, 0.8f);
		umaDna.jawsPosition = Random.Range (0.3f, 0.8f);

		umaDna.cheekSize = Random.Range (0.3f, 0.8f);
		umaDna.cheekPosition = Random.Range (0.3f, 0.8f);
		umaDna.lowCheekPronounced = Random.Range (0.3f, 0.8f);
		umaDna.lowCheekPosition = Random.Range (0.3f, 0.8f);

		umaDna.foreheadSize = Random.Range (0.3f, 0.8f);
		umaDna.foreheadPosition = Random.Range (0.15f, 0.65f);

		umaDna.lipsSize = Random.Range (0.3f, 0.8f);
		umaDna.mouthSize = Random.Range (0.3f, 0.8f);
		umaDna.eyeRotation = Random.Range (0.3f, 0.8f);
		umaDna.eyeSize = Random.Range (0.3f, 0.8f);
		umaDna.breastSize = Random.Range (0.3f, 0.8f);
	}
}
