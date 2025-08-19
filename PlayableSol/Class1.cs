using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using TrailsFX;

[BepInPlugin("PlayableSol", "Swaps Sen's model with Sol's.", "1.0.0")]
public class SinModelSwap : BaseUnityPlugin
{
    private GameObject senHead;
    private GameObject solHead;
    private GameObject senHumanbot;
    private GameObject solHumanbot;
    private GameObject senUpperCape;
    private GameObject senLowerCape;
    private GameObject senNeck;
    private GameObject senCorrupt;
    private GameObject justSen;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BirdCage")
        {
            Debug.Log("DaDemo scene loaded, starting model swap process.");
            FindGameObjects();
            if (senHumanbot != null && solHumanbot != null && senUpperCape != null && senLowerCape != null && senNeck != null && senCorrupt != null)
            {
                PerformModelSwap();
            }
        }
    }

    void FindGameObjects()
    {
        senHumanbot = GameObject.Find("S-105.1/Humanbot_A_Geom");
        solHumanbot = GameObject.Find("World/Areas/ScrapObservatory/Sol/Humanbot_A_Geom");
        senUpperCape = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/UpperCape");
        senLowerCape = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/UpperCape/LowerCape");
        senNeck = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/Neck");
        senCorrupt = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/Corrupt");
        justSen = GameObject.Find("S-105.1");
    }

    void PerformModelSwap()
    {
        ReplaceMaterials(senHumanbot, solHumanbot);
        ReplaceSharedMesh(senHumanbot, solHumanbot);
        DisableSenCorrupt();
        DisableUpperCape();
        DisableLowerCape();
        CopyTopsMaterial();
        CopyAndPlaceTops();
        UpdateTrailsFXColor();
        InstantiateSinCorruptHead();
        InstantiateSolAccessories();
        Debug.Log("Model swap process completed successfully.");
    }

    void InstantiateSinCorruptHead()
    {
        GameObject sinCorruptHead = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Neck/Pivot/Sphere");
        if (sinCorruptHead == null) return;
        GameObject newCorruptHead = (GameObject)UnityEngine.Object.Instantiate(sinCorruptHead);
        newCorruptHead.name = "Corrupt(Clone)";
        Transform senSpine1 = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1")?.transform;
        if (senSpine1 == null) return;
        newCorruptHead.transform.SetParent(senSpine1, false);
        newCorruptHead.transform.localPosition = new Vector3(0f, 0.4067f, -0.0272f);
        newCorruptHead.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        newCorruptHead.transform.localScale = new Vector3(0.3153f, 0.3153f, 0.3153f);
    }

    void InstantiateSolAccessories()
    {
        Transform senSpine1 = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1")?.transform;
        if (senSpine1 == null) return;
        Transform senNeckPivot = senNeck != null ? senNeck.transform.Find("Pivot") : null;
        Transform preferredHeadParent = senNeckPivot ?? senSpine1.Find("Neck/Pivot") ?? senSpine1;
        GameObject solSphere = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Neck/Pivot/Sphere (1)");
        if (solSphere != null)
        {
            GameObject newSphere = (GameObject)UnityEngine.Object.Instantiate(solSphere);
            newSphere.name = solSphere.name + "(Clone)";
            BakeSkinnedToStatic(newSphere);
            var fastLights = newSphere.GetComponentsInChildren<UL_FastLight>(true);
            foreach (var fl in fastLights) UnityEngine.Object.Destroy(fl);
            newSphere.transform.SetParent(preferredHeadParent, true);
            newSphere.transform.localPosition = new Vector3(0f, 0.44f, 0.108f);
            newSphere.transform.localRotation = solSphere.transform.localRotation;
            newSphere.transform.localScale = new Vector3(0.0325f, 0.0433f, 0.0075f);
        }
        GameObject solUpperCape = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Neck/UpperCape");
        if (solUpperCape != null)
        {
            GameObject newUpperCape = (GameObject)UnityEngine.Object.Instantiate(solUpperCape);
            newUpperCape.name = solUpperCape.name + "(Clone)";
            BakeSkinnedToStatic(newUpperCape);
            newUpperCape.transform.SetParent(senSpine1, false);
            newUpperCape.transform.localPosition = new Vector3(0.0011f, -2.33f, 0.79f);
            newUpperCape.transform.localRotation = solUpperCape.transform.localRotation;
            newUpperCape.transform.localScale = new Vector3(1.5119f, 1.4446f, 1.8041f);
        }
        GameObject solPanel = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Neck/Pivot/SM_Prop_SolarPanel_01_Panel_02 (1)");
        if (solPanel != null)
        {
            GameObject newPanel = (GameObject)UnityEngine.Object.Instantiate(solPanel);
            newPanel.name = solPanel.name + "(Clone)";
            BakeSkinnedToStatic(newPanel);
            Transform shoulderParent = senSpine1.Find("Shoulder_R") ?? senSpine1;
            newPanel.transform.SetParent(shoulderParent, true);
            newPanel.transform.localPosition = new Vector3(0f, 0.49f, -0.23f);
            newPanel.transform.localRotation = Quaternion.Euler(340.5706f, 224.6809f, 253.2408f);
            newPanel.transform.localScale = new Vector3(0.1134f, 0.1134f, 0.1134f);
        }
        GameObject solPanelAlt = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Neck/Pivot/SM_Prop_SolarPanel_01_Panel_01 (1)");
        if (solPanelAlt != null)
        {
            GameObject newPanelAlt = (GameObject)UnityEngine.Object.Instantiate(solPanelAlt);
            newPanelAlt.name = solPanelAlt.name + "(Clone)";
            BakeSkinnedToStatic(newPanelAlt);
            Transform shoulderParentAlt = senSpine1.Find("Shoulder_L") ?? senSpine1;
            newPanelAlt.transform.SetParent(shoulderParentAlt, true);
            newPanelAlt.transform.localPosition = new Vector3(0f, 0.49f, -0.23f);
            newPanelAlt.transform.localRotation = Quaternion.Euler(340.5706f, 224.6809f, 253.2408f);
            newPanelAlt.transform.localScale = new Vector3(0.1134f, 0.1134f, 0.1134f);
        }
    }

    void BakeSkinnedToStatic(GameObject go)
    {
        var smrList = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var smr in smrList)
        {
            if (smr == null || smr.sharedMesh == null) continue;
            Mesh baked = new Mesh();
            smr.BakeMesh(baked);
            GameObject meshHolder = smr.gameObject;
            var existingFilter = meshHolder.GetComponent<MeshFilter>();
            if (existingFilter == null) existingFilter = meshHolder.AddComponent<MeshFilter>();
            existingFilter.mesh = baked;
            var existingRenderer = meshHolder.GetComponent<MeshRenderer>();
            if (existingRenderer == null) existingRenderer = meshHolder.AddComponent<MeshRenderer>();
            existingRenderer.sharedMaterials = smr.sharedMaterials;
            UnityEngine.Object.Destroy(smr);
        }
    }

    void ReplaceMaterials(GameObject senObj, GameObject solObj)
    {
        SkinnedMeshRenderer senRenderer = senObj?.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer solRenderer = solObj?.GetComponent<SkinnedMeshRenderer>();
        if (senRenderer == null || solRenderer == null) return;
        Material solMaterial = solRenderer.material;
        if (solMaterial == null) return;
        senRenderer.material = (Material)UnityEngine.Object.Instantiate(solMaterial);
    }

    void ReplaceSharedMesh(GameObject senObj, GameObject solObj)
    {
        SkinnedMeshRenderer senRenderer = senObj?.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer solRenderer = solObj?.GetComponent<SkinnedMeshRenderer>();
        if (senRenderer == null || solRenderer == null) return;
        Mesh solMesh = solRenderer.sharedMesh;
        if (solMesh == null) return;
        senRenderer.sharedMesh = solMesh;
    }

    void CopyTopsMaterial()
    {
        GameObject solTops = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Frank_RPGSpear_Cape");
        if (solTops == null) return;
        SkinnedMeshRenderer solTopsRenderer = solTops.GetComponent<SkinnedMeshRenderer>();
        if (solTopsRenderer == null) return;
        Material solTopsMaterial = solTopsRenderer.material;
        if (solTopsMaterial == null) return;
        UpdateCapeMaterial(senUpperCape, solTopsMaterial);
        UpdateCapeMaterial(senLowerCape, solTopsMaterial);
    }

    void UpdateCapeMaterial(GameObject capeObject, Material newMaterial)
    {
        if (capeObject == null || newMaterial == null) return;
        MeshRenderer capeRenderer = capeObject.GetComponent<MeshRenderer>();
        if (capeRenderer == null) return;
        capeRenderer.material = (Material)UnityEngine.Object.Instantiate(newMaterial);
    }

    void CopyAndPlaceTops()
    {
        GameObject solTops = GameObject.Find("World/Areas/ScrapObservatory/Sol/ROOT/Hips/Spine/Spine1/Frank_RPGSpear_Cape");
        if (solTops == null) return;
        GameObject newTops = (GameObject)UnityEngine.Object.Instantiate(solTops);
        newTops.name = "Tops_14(Clone)";
        Transform senSpine1 = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1")?.transform;
        if (senSpine1 == null) return;
        newTops.transform.SetParent(senSpine1, false);
        newTops.transform.localPosition = solTops.transform.localPosition;
        newTops.transform.localRotation = solTops.transform.localRotation;
    }

    void DisableSenCorrupt()
    {
        if (senCorrupt == null) return;
        Renderer corruptRenderer = senCorrupt.GetComponent<Renderer>();
        if (corruptRenderer != null) corruptRenderer.enabled = false;
        Transform corruptTransform = senCorrupt.transform;
        Transform eyeActiveTransform = corruptTransform.Find("EyeActive");
        if (eyeActiveTransform != null) eyeActiveTransform.gameObject.SetActive(false);
    }

    void DisableUpperCape()
    {
        if (senUpperCape == null) return;
        Renderer upperCapeRenderer = senUpperCape.GetComponent<Renderer>();
        if (upperCapeRenderer != null) upperCapeRenderer.enabled = false;
    }

    void DisableLowerCape()
    {
        if (senLowerCape == null) return;
        Renderer lowerCapeRenderer = senLowerCape.GetComponent<Renderer>();
        if (lowerCapeRenderer != null) lowerCapeRenderer.enabled = false;
    }

    void UpdateTrailsFXColor()
    {
        var trailComponent = senHumanbot?.GetComponent<TrailEffect>();
        if (trailComponent == null) return;
        trailComponent.enabled = true;
        trailComponent.active = true;
        trailComponent.color = new Color(0f, 0f, 0f, 1f);
    }
}
