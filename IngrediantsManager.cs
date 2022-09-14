using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XStudios.RFID;
using Zenject;

// This code was part of the cookie maker project we did for Gaylord Resort hotels’ “Elf Save the Christmas” attraction, using Unity3d, C# and MQTT messaging. 
// Visitors would physically select cookie ingrediants (using RFID) and in the monitor, their completed cookie will show up. 
// Also look at the cookie class. 
// video of the a visitor experiencing the project can be seen here (watch at time 5:40) : https://youtu.be/Z6vvgUuGKc8?t=336 

namespace XStudios
{
    public class IngrediantsManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _ingrediantSprites;
        [SerializeField]
        private Material[] _ingrediantMats;
        [SerializeField]
        private Material[] _cookieMats;
        [SerializeField]
        private MeshRenderer[] _meshRenderers;
        [SerializeField]
        private Image[] _images;
        [SerializeField]
        private GameObject[] _checkmarks;
        [SerializeField]
        private TextMeshProUGUI[] _labels;
        private Cookie _cookie;
        private int _counter = 0;
        private int _totalNumber;
        private int _cookieType;
        public Material _cookieMat;
        private Dictionary<int, int> _ingrediantByIndex;
        private RfidDatabaseControllerJson _rfidDatabaseController;
        private RfidDataProcessor _rfidDataProcessor;
        private List<RfidTypeFood> _rfidTypeFoods = new List<RfidTypeFood>();
        private IngrediantsScreen _ingrediantScreen;
        private float _ingrediantsTimer;

        private static IngrediantsManager _instance;

        public static IngrediantsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<IngrediantsManager>();
                    _instance.Init();
                }
                return _instance;
            }
        }

        [Inject]
        private void Construct(RfidDatabaseControllerJson rfidDatabaseController,
            RfidDataProcessor rfidDataProcessor)
        {
            _rfidDatabaseController = rfidDatabaseController;
            _rfidDataProcessor = rfidDataProcessor;
        }

        public void ResetIngrediantTimer()
        {
            _ingrediantsTimer = Time.time;
        }

        private void Init()
        {
            _ingrediantScreen = GetComponent<IngrediantsScreen>();
        }

        public void SetCookie(Cookie cookie, int matNumber)
        {
            _cookie = cookie;
            _cookieType = matNumber - 1;
            _cookieMat = _cookieMats[matNumber - 1];
            _ingrediantByIndex = new Dictionary<int, int>();
            List<int> flours = _cookie.GetFlours();
            for (int i = 0; i < 3; i++)
            {
                _images[i].sprite = _ingrediantSprites[flours[i] - 1];
                _images[i].gameObject.SetActive(true);
                string ingrediant = ((Cookie.Ingrediants)flours[i]).ToString();
                if (ingrediant.Equals("BakingSoda"))
                    ingrediant = "Baking Soda";
                _labels[i].text = ingrediant;
                _ingrediantByIndex.Add(flours[i], i);
            }
        }

        public Material GetCookieMat()
        {
            return _cookieMat;
        }

        public int GetCookieType()
        {
            return _cookieType;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
                RFIDReceived(1);
            if (Input.GetKeyUp(KeyCode.Alpha2))
                RFIDReceived(2);
            if (Input.GetKeyUp(KeyCode.Alpha3))
                RFIDReceived(3);
            if (Input.GetKeyUp(KeyCode.Alpha4))
                RFIDReceived(4);
            if (Input.GetKeyUp(KeyCode.Alpha5))
                RFIDReceived(5);

            if (ScreenHandler.Instance.CurrentScreen.GetType() == typeof(IngrediantsScreen))
            {
                CheckNewRfid();
                if (Time.time - _ingrediantsTimer > ConfigLoader.Instance.GetConfig().ingrediant_timeout)
                {
                    CheckNextIngrediant();
                    _ingrediantsTimer = Time.time;
                }
            }
        }


        void CheckNextIngrediant()
        {
            for (int i = 0; i < 3; i++)
            {
                if (!_checkmarks[i].activeInHierarchy)
                {
                    int ingrediant = GetIngrediantNumber(i);
                    RFIDReceived(ingrediant);
                    break;
                }
            }
        }


        int GetIngrediantNumber(int index)
        {
            foreach(KeyValuePair<int, int> pair in _ingrediantByIndex)
            {
                if (pair.Value == index)
                    return pair.Key;
            }
            return -1;
        }

        private void CheckNewRfid()
        {
            foreach (var tag in _rfidDataProcessor.HeldTags)
            {
                _ingrediantScreen.ResetInactivityTime();
                if(!_rfidTypeFoods.Contains((RfidTypeFood)_rfidDatabaseController.RequestRfidType(tag.Key)))
                {
                    _rfidTypeFoods.Add((RfidTypeFood)_rfidDatabaseController.RequestRfidType(tag.Key));
                    int toyToNumber = Convert.ToInt32(_rfidDatabaseController.RequestRfidType(tag.Key));
                    string toyToString = Convert.ToString(toyToNumber);
                    RFIDReceived(toyToNumber);
                }
            }
        }

        void RFIDReceived(int number)
        {
            if (_ingrediantByIndex.ContainsKey(number) && !_checkmarks[_ingrediantByIndex[number]].activeInHierarchy)
            {
                _ingrediantsTimer = Time.time;
                _checkmarks[_ingrediantByIndex[number]].SetActive(true);
                _meshRenderers[_counter].material = _ingrediantMats[number - 1];
                _meshRenderers[_counter].gameObject.SetActive(true);
                _totalNumber |= number;
                _counter++;
                if (_counter == 3)
                {
                    StartCoroutine(NextScreen());
                }
            }
        }


        IEnumerator NextScreen()
        {
            yield return new WaitForSeconds(0.4f);
            ScreenHandler.Instance.NextScreen();
        }

        public void ResetMeshRenderers()
        {
            foreach (MeshRenderer renderer in _meshRenderers)
                renderer.gameObject.SetActive(false);
        }


        public void Reset()
        {
            _totalNumber = 0;
            foreach (Image image in _images)
                image.sprite = null;
            foreach (GameObject check in _checkmarks)
                check.SetActive(false);
            foreach (TextMeshProUGUI text in _labels)
                text.text = "";
            /*foreach (MeshRenderer renderer in _meshRenderers)
                renderer.gameObject.SetActive(false);*/
            _ingrediantByIndex.Clear();
            _counter = 0;
            _rfidTypeFoods.Clear();
        }
    }
}


