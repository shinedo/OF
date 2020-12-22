using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arithmetic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RemoveElement(int[] nums, int val)
    {
        int i = 0;
        for (int j = 0; j < nums.Length; j++)
        {
            if (nums[j] != val)
            {
                nums[i] = nums[j];
                i++;
            }
        }

        return i;
    }

    public int RemoveElement2(int[] nums, int val)
    {
        int i = 0;
        int length = nums.Length;
        for (; i<length; )
        {
            if (nums[i] == val)
            {
                nums[i] = nums[length - 1];
                length--;
            }
            else
                i++;
        }

        return length;
    }
    
}
