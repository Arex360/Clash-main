using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
public class CustomNetworkDiscovery : NetworkDiscoveryHUD
{
    public GameObject menu;
    public bool isServer;
    ServerResponse info;
    public bool notJoined;
    long key;
    int index = 0;
    public void FindServers(){
        discoveredServers.Clear();
        print("starting discorvery");
        networkDiscovery.StartDiscovery();
        int index = 0;
        foreach (ServerResponse info in discoveredServers.Values){
            print(info.uri);
        }
        //info = discoveredServers;
        //foreach(KeyValuePair<long , ServerResponse> pair in discoveredServers){
          //  key = pair.Key;
        //}

    }
    public void Update(){
        if (!menu)
        {
            menu = FindObjectOfType<MenuController>().gameObject;
        }
       if(discoveredServers.Count > 0 ){
            foreach (ServerResponse info in discoveredServers.Values){
            print(info.uri);
            NetworkManager.singleton.StartClient(info.uri);
            if(menu){
                menu.SetActive(false);
            }
            if(notJoined){
                networkDiscovery.StopDiscovery();
                print("joining server");
                NetworkManager.singleton.StartClient(info.uri);
                print("joined");
                notJoined = false; 
            }
        }
       }
        
    }
    public void ConnectToServer(ServerResponse info){
        networkDiscovery.StopDiscovery();
        print("connectingh");
        NetworkManager.singleton.StartClient(info.uri);
       // print("conneted");
        menu.SetActive(false);
    }
    public void Host(){
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        menu.SetActive(false);
        isServer = true;
    }
    public  void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }
}
