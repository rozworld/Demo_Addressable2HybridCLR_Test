using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<AssetReference> assetReferences = new List<AssetReference>();
    public AssetReference loginScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGameScene(int index)
    {
        Addressables.LoadSceneAsync(assetReferences[index], LoadSceneMode.Single);
    }

    public void ExitAddressablesScene()
    {
        if (loginScene != null)
            Addressables.LoadSceneAsync(loginScene, LoadSceneMode.Single);
    }

}
