using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour 
    //�̱��� ��������, �̺�Ʈ�� ���.����.Ʈ�����ϴ� ���
    //�̺�Ʈ �Ŵ����� �̺�Ʈ�� Ű�� ����ϰ� �ش� �̺�Ʈ�� �����ϴ� �ڵ鷯(Ŀ�ǵ�)�� ����Ʈ�� 
    //������ ����ϴ� ��ųʸ��� ���������� �����Ѵ�. �̺�Ʈ�� �߻���ų ������ ��ųʸ��� ��ȸ�Ͽ� �ش� �̺�Ʈ�� ��ϵ� ��� �ڵ鷯�� �����Ѵ�.
{
    //�̱��� �ν��Ͻ�
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // �̺�Ʈ �Ŵ����� �������� �ʴ� ���, ���� ����
                var obj = new GameObject("EventManager");
                _instance = obj.AddComponent<EventManager>();
            }
            return _instance;
        }
    }

    //�̺�Ʈ ��ųʸ�
   // private Dictionary<Type, Action<EventBase>> eventDictionary;


    //�ν��Ͻ��� �ʱ�ȭ�� ��ųʸ��� �ʱ�ȭ�� ����
    void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            //eventDictionary = new Dictionary<Type, Action<EventBase>>();
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Ư�� �̺�Ʈ Ÿ�Կ� ���� �����ʸ� ���
    
    /*
    public void Subscribe<T>(Action<T> listener) where T : EventBase
    {
        Type eventType = typeof(T);
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = (x => listener((T)x));
        }
        else
        {
            eventDictionary[eventType] += (x => listener((T)x));
        }
    }

    public void Unsubscribe<T>(Action<T> listener) where T : EventBase
    {
        Type eventType = typeof(T);
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= (x => listener((T)x));
        }
    }

    public void TriggerEvent(EventBase eventToTrigger)
    {
        Type eventType = eventToTrigger.GetType();
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType].Invoke(eventToTrigger);
        }
    }
    
    */
}

public abstract class EventBase { }
