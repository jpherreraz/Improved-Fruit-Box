using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apple : MonoBehaviour
{   
    [SerializeField]
    private Text text;
    private int value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int val) {
        this.value = val;
        text.text = val.ToString();
    }

    public int GetValue() {
        return value;
    }
}
