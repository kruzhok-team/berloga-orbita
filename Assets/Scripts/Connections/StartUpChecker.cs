using Connections;
using UnityEngine;

public class StartUpChecker : MonoBehaviour
{
    private ConnectionsModule module = new ConnectionsModule();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       StartCoroutine(module.GetServerInfo());
       StartCoroutine(module.GetDevices());
       StartCoroutine(module.GetParameters());
       StartCoroutine(module.PostCalculation("planets", "<?xml version=\"1.0\" encoding=\"utf-8\"?><v:testmodel name=\"test\" xmlns:v=\"venus\"><planet>Mars</planet><tick>10</tick><square>10.0</square><mass>100.0</mass><h>50000</h><x>0</x><vy>0</vy><vx>5000</vx><aerodynamic_coeff>0.47</aerodynamic_coeff></v:testmodel>"));

       StartCoroutine(module.GetCalculationStatus("planets", "1734519806636257"));
       StartCoroutine(module.GetCalculationStatus("planets", "1734519806780284"));
       StartCoroutine(module.GetCalculationResult("planets", "1734519806636257"));
       StartCoroutine(module.GetCalculationResult("planets", "1734519806780284"));
       
       Debug.Log("what");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
