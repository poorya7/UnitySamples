using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;


// loads products from the cloud using addressables

public class ProductLoader : MonoBehaviour
{
    public static readonly Dictionary<string, GameObject> m_productsByName = new Dictionary<string, GameObject>();

    public static ProductLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadProductsFromCloud();

    }


    async void LoadProductsFromCloud()
    {
        if (m_productsByName.Count == 0)
        {
            await InitProducts("Product");
        }
        AddProductsToRoom();
    }


    void AddProductsToRoom()
    {
        if (CmsData.HasData())
        {
            Room room = CmsData.GetRoomByName(SceneManager.GetActiveScene().name);
            foreach (Product product in room.GetProducts())
            {
                if (m_productsByName.ContainsKey(product.m_internalName))
                {
                    GameObject p = Instantiate(m_productsByName[product.m_internalName], TapDetector.Instance.GetRoom().transform);
                    SetProductFxLayer(p);
                    p.transform.position = product.m_position;
                    p.transform.eulerAngles = product.m_rotation;
                    float scale = product.m_scaleRoom;
                    p.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }


    public static async Task InitProducts(string assetLabel)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(assetLabel, typeof(GameObject)).Task;
        List<Task<GameObject>> tasks = new List<Task<GameObject>>();

        foreach (var location in locations)
            tasks.Add(Addressables.LoadAssetAsync<GameObject>(location).Task);

        var loadedProducts = await Task.WhenAll(tasks);

        //Room room = CmsData.GetRoomByName(SceneManager.GetActiveScene().name);
        foreach (var product in loadedProducts)
        {
            if (CmsData.ProductExists(product.name))
                m_productsByName.Add(product.name, product);
            else
                Debug.Log("******* Product " + product.name + " doesn't exist.");
        }
        Debug.Log("Loaded " + m_productsByName.Count + " products.");
    }


    public GameObject GetProductByName(string name)
    {
        if (m_productsByName.ContainsKey(name))
            return m_productsByName[name];
        else
            return null;
    }
}
