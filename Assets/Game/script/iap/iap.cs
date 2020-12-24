using System;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class iap : MonoBehaviour, IStoreListener
{
    private IAppleExtensions m_AppleExtensions;

#if IAPWORLD
    const string starblade_crystal_120 = "starblade_world_crystal_120";
    const string starblade_crystal_600 = "starblade_world_crystal_600";
    const string starblade_crystal_1360 = "starblade_world_crystal_1360";
    const string starblade_crystal_2560 = "starblade_world_crystal_2560";
    const string starblade_crystal_6560 = "starblade_world_crystal_6560";

    const string starblade_vip = "starblade_world_vip";
    const string starblade_RemoveInterAd = "starblade_world_RemoveAD";
#else
    const string starblade_crystal_120 = "starblade_crystal_120";
    const string starblade_crystal_600 = "starblade_crystal_600";
    const string starblade_crystal_1360 = "starblade_crystal_1360";
    const string starblade_crystal_2560 = "starblade_crystal_2560";
    const string starblade_crystal_6560 = "starblade_crystal_6560";

    const string starblade_vip = "starblade_vip";
    const string starblade_RemoveInterAd = "starblade_RemoveAD";
#endif
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public string result = "nothing";
    // Start is called before the first frame update
    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }

    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        builder.AddProduct(starblade_crystal_120, ProductType.Consumable);
        builder.AddProduct(starblade_crystal_600, ProductType.Consumable);
        builder.AddProduct(starblade_crystal_1360, ProductType.Consumable);
        builder.AddProduct(starblade_crystal_2560, ProductType.Consumable);
        builder.AddProduct(starblade_crystal_6560, ProductType.Consumable);

        builder.AddProduct(starblade_vip, ProductType.Subscription);
        builder.AddProduct(starblade_RemoveInterAd, ProductType.NonConsumable);


        // Continue adding the non-consumable product.
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 
        // builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
        //         { kProductNameAppleSubscription, AppleAppStore.Name },
        //         { kProductNameGooglePlaySubscription, GooglePlay.Name },
        //     });

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        result = "init...";
        UnityPurchasing.Initialize(this, builder);

    }

    public void OnPurchseFinished(Product P)
    {
    }


    public void Btn_Buy(int ID)
    {
        gDefine.PlaySound(57);
        if (ID == 5)
        {
            if (PlayerPrefs.GetInt("vip", 0) == 1)
            {
                Debug.Log("已经是会员了，直接返回");
                gDefine.gBoxData.VipActive();
                return;
            }
        }
        else if(ID == 6)
        {
            if (PlayerPrefs.GetInt("removeInterAD", 0) == 1)
            {
                Debug.Log("已经买过去广告");
                return;
            }
        }

        result = "before buy :" + ID;
        if (IsInitialized())
        {
            string IDStr = "";
            switch(ID)
            {
                case 0:
                    IDStr = starblade_crystal_120;
                    break;
                  case 1:
                    IDStr = starblade_crystal_600;
                    break; 
                     case 2:
                    IDStr = starblade_crystal_1360;
                    break;
                      case 3:
                    IDStr = starblade_crystal_2560;
                    break;
                      case 4:
                    IDStr = starblade_crystal_6560;
                    break;
                      case 5:
                    IDStr = starblade_vip;
                    break;
                    case 6:
                    IDStr = starblade_RemoveInterAd;
                    break;
            }
            result = "buy :" + ID;
            m_StoreController.InitiatePurchase(IDStr);
        }
        result = "end buy ";

    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        result = "OnInitialized: PASS";

        // PlayerPrefs.SetInt("vip", 0);
       //  gDefine.gBoxData.VipCancel();

        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                if (item.receipt != null)
                {
                    if (item.definition.type == ProductType.Subscription)
                    {
                        if (checkIfProductIsAvailableForSubscriptionManager(item.receipt))
                        {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();

                            if (info.isExpired() == Result.True || info.isCancelled() == Result.True)
                            {
                                if( info.getProductId() == starblade_RemoveInterAd)
                                {
                                     PlayerPrefs.SetInt("removeInterAD", 0);
                                }
                                else if( info.getProductId() == starblade_vip)
                                {
                                    PlayerPrefs.SetInt("vip", 0);
                                    gDefine.gBoxData.VipCancel();
                                }
                                
                            }

                            // Debug.Log("product id is: " + info.getProductId());
                            // Debug.Log("purchase date is: " + info.getPurchaseDate());
                            // Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            // Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            // Debug.Log("is expired? " + info.isExpired().ToString());
                            // Debug.Log("is cancelled? " + info.isCancelled());
                            // Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            // Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            // Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            // Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            // Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            // Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            // Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
                        }
                        else
                        {
                            Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }
                    }
                    else
                    {
                        Debug.Log("the product is not a subscription product");
                    }
                }
                else
                {
                    Debug.Log("the product should have a valid receipt");
                }


            }
        }

    }

    private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
    {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
        {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper["Store"];
        var payload = (string)receipt_wrapper["Payload"];

        if (payload != null)
        {
            switch (store)
            {
                case GooglePlay.Name:
                    {
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                            return false;
                        }
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                            return false;
                        }
                        var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                        var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                        if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                            return false;
                        }
                        return true;
                    }
                case AppleAppStore.Name:
                case AmazonApps.Name:
                case MacAppStore.Name:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        return false;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        result = "OnInitializeFailed InitializationFailureReason:" + error;
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        result = string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id);

        if (String.Equals(args.purchasedProduct.definition.id, starblade_crystal_120, StringComparison.Ordinal))
        {
            gDefine.gPlayerData.Crystal += 1260;
            gDefine.gMainGainTip.Show(202, 1200 + 60);
            gDefine.PlaySound(57);
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            //ScoreManager.score += 100;
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_crystal_600, StringComparison.Ordinal))
        {
            gDefine.gPlayerData.Crystal += 6600;
            gDefine.gMainGainTip.Show(202, 6000 + 600);
            gDefine.PlaySound(57);
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_crystal_1360, StringComparison.Ordinal))
        {
            gDefine.gPlayerData.Crystal += 15640;
            gDefine.gMainGainTip.Show(202, 13600 + 2040);
            gDefine.PlaySound(57);
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_crystal_2560, StringComparison.Ordinal))
        {
            gDefine.gPlayerData.Crystal += 30720;
            gDefine.gMainGainTip.Show(202, 25600 + 5120);
            gDefine.PlaySound(57);
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_crystal_6560, StringComparison.Ordinal))
        {
            gDefine.gPlayerData.Crystal += 82000;
            gDefine.gMainGainTip.Show(202, 65600 + 16400);
            gDefine.PlaySound(57);
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_vip, StringComparison.Ordinal))
        {

            if (PlayerPrefs.GetInt("vip", 0) == 0)
            {
                PlayerPrefs.SetString("vipT", "0");
            }
            PlayerPrefs.SetInt("vip", 1);
            PlayerPrefs.Save();
            gDefine.ShowTip(gDefine.GetStr(430));

            gDefine.gBoxData.VipActive();
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, starblade_RemoveInterAd, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("removeInterAD", 1);
            PlayerPrefs.Save();
            gDefine.ShowTip(gDefine.GetStr(459));
        }


        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
    }

    private void OnTransactionsRestored(bool success)
    {
        if (success)
            gDefine.ShowTip(gDefine.GetStr(429));
        Debug.Log("Transactions restored." + success);
    }

    public void RestoreButtonClick()
    {
        gDefine.PlaySound(57);
        m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);

    }
}
