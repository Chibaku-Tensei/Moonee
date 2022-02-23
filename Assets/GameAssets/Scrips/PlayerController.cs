using Base;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class PlayerController : BaseMono
    {
        [SerializeField] private Transform currencyParent;
        private Rigidbody _body;

        private int _height = 5;

        private int _index = 0;
        
        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Currency 1") && !other.GetComponent<CurrencyItem>().Ignore)
            {
                PickUpCurrency(other.transform);
                other.GetComponent<CurrencyItem>().Ignore = true;
            }
            else if (other.CompareTag("Construction"))
            {
                DropCurrency(other.transform, 6);
            }
        }

        private void PickUpCurrency(Transform currencyTransform)
        {
            currencyTransform.SetParent(currencyParent);
            Sequence t = DOTween.Sequence();
            t.Join(currencyTransform.DOLocalMove(new Vector3(0, (_index % 5) * .275f, 0.525f * -(_index / 5)), .5f).SetEase(Ease.InSine));
            t.Join(currencyTransform.DOLocalRotate(Vector3.zero, .5f));
            t.Play();

            _index++;
        }

        private void DropCurrency(Transform destination, int amount)
        {
            Vector3 pos = destination.position;
            Sequence t = DOTween.Sequence();

            var childCount = currencyParent.childCount;
            int length = amount > childCount ? childCount : amount;
            
            for (int i = 0; i < length; i++)
            {
                var child = currencyParent.GetChild(0);
                child.parent = null;
                t.Join(child.DOMove(pos, .75f).SetEase(Ease.OutCubic));
            }

            t.Play();
        }

        private void ReArrangeCurrency()
        {
            
        }

        public void CharacterMove(Vector3 motion)
        {
            _body.velocity = motion;
        }
    } 
}

