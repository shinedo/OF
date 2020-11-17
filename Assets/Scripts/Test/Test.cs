using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Person p = new Person();
        p.Age = 2;
        p.Id = 3;
        p.Name = "Shinedo";

        byte[] dataBytes = p.ToByteArray();
        
        IMessage imPerson = new Person(); 
        Person p1 =new Person();
        p1 = (Person) imPerson.Descriptor.Parser.ParseFrom(dataBytes);

        Debug.Log(p1.Age);
        Debug.Log(p1.Id);
        Debug.Log(p1.Name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
