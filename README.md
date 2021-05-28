# Unity-Endless-Prison-Prototype

When I saw the Manifold Garden, I liked its concept very much. No matter what you did, you were going back to the same place. It's as if you have fallen into an endless prison. I wanted to design a system similar to this one.

[Manifold Garden][garden]'ı görünce konseptini çok beğendim. ne yaparsan yap aynı yere geri dönüyordun. Sanki sonsuz bir hapishanenin içine düşülmüş gibi. Buna benzer bir sistem tasarlamak istedim.

ilk olarak bu sonsuzluk kavramını nasıl işleyeceğimi düşündüm. Karakteri haritanın sonuna geldiğinde, haritanın başına olması gerektiği yere ışınlayarak bunu sağlama kararı aldım. Bu mekanik üzerine düşünerek yapılacaklar listemizi oluşturdum:
* Test alanının oluşturulması
* Kameranın sonsuzmuş gibi görmesi
* Haritanın otomatik oluşması
* Karakterin ışınlanması

## Test alanının oluşturulması
Unitydeki küpleri kullanarak aşağıdaki gibi basit bir alan oluşturdum. Etrafında `Collider`'lar bulunuyor. Bu Colliderlar test alanının sınırları, yani oluşturulan hapishane hücresinin duvarları olarak düşünülebilir.

![resim1](https://ergulburak.github.io/assets/img/sonsuzluk-1.PNG)

## Kameranın sonsuzmuş gibi görmesi
Kamerada sonsuzmuş gibi görünmesi için haritanin kopyalarını üç boyutlu bir matris oluşturarak merkezde ana harita bulunacak şekilde oluşturdum.

![resim2](https://ergulburak.github.io/assets/img/sonsuzluk-3.PNG) 

Kameraya `fog` vererek uzak yerlerin sis yüzünden görülmemesini sağladım. 

![resim3](https://ergulburak.github.io/assets/img/sonsuzluk-4.PNG)

Bu sayede sanki sonsuza kadar tekrar eden bir alana bakıyormuş hissiyatı oluştu.

![resim4](https://ergulburak.github.io/assets/img/sonsuzluk-5.PNG)

## Haritanın otomatik oluşması
Her sahne için haritayı böyle ayarlamak çok zaman kaybettireceği için üretici bir script hazırladım.

![resim5](https://ergulburak.github.io/assets/img/sonsuzluk-6.PNG)

Yukarıdaki girdileri verince üç boyutlu matris haritamızı oluşturup gerekli `Collider`'ları `tag`'lerini verek oluşturuyor.
```csharp
public void Generate(){
    for (int x = 0; x < repeatValue * 2 + 1; x++)
    {
        for (int y = 0; y < repeatValue * 2 + 1; y++)
        {
            for (int z = 0; z < repeatValue * 2 + 1; z++)
            {
                GameObject map = Instantiate(mapPrefab, mapParent.transform);
                map.transform.position = new Vector3(
                    (mapWidth * x)-(((repeatValue * 2 + 1) * mapWidth)/2), 
                    (mapHeight * y)-(((repeatValue * 2 + 1) * mapHeight)/2), 
                    (mapDepth * z)-(((repeatValue * 2 + 1) * mapDepth)/2));
                        
                if (x == (repeatValue * 2 + 1) / 2 && 
                    y == (repeatValue * 2 + 1) / 2 &&
                    z == (repeatValue * 2 + 1) / 2)
                {
                    GameObject collider = Instantiate(empty);
                    collider.transform.position=Vector3.zero;
                    collider.name = "Collider";

                    GameObject forward = Instantiate(empty);
                    forward.name = "Forward";
                    forward.tag = "Forward";
                    forward.transform.position = new Vector3(0, 0, mapDepth / 2);
                    forward.transform.parent = collider.transform;
                    BoxCollider boxColliderForward = forward.AddComponent<BoxCollider>();
                    boxColliderForward.isTrigger = true;
                    boxColliderForward.size = new Vector3(mapWidth,mapHeight,.5f);

                    GameObject back = Instantiate(empty);
                    back.name = "Back";
                    back.tag = "Back";
                    back.transform.position = new Vector3(0, 0, -mapDepth / 2);
                    back.transform.parent = collider.transform;
                    BoxCollider boxColliderBack = back.AddComponent<BoxCollider>();
                    boxColliderBack.isTrigger = true;
                    boxColliderBack.size = new Vector3(mapWidth,mapHeight,.5f);

                    GameObject right = Instantiate(empty);
                    right.name = "Right";
                    right.tag = "Right";
                    right.transform.position = new Vector3(mapWidth/2, 0, 0);
                    right.transform.parent = collider.transform;
                    BoxCollider boxColliderRight = right.AddComponent<BoxCollider>();
                    boxColliderRight.isTrigger = true;
                    boxColliderRight.size = new Vector3(.5f,mapHeight,mapDepth);

                    GameObject left = Instantiate(empty);
                    left.name = "Left";
                    left.tag = "Left";
                    left.transform.position = new Vector3(-mapWidth/2, 0, 0);
                    left.transform.parent = collider.transform;
                    BoxCollider boxColliderLeft = left.AddComponent<BoxCollider>();
                    boxColliderLeft.isTrigger = true;
                    boxColliderLeft.size = new Vector3(.5f,mapHeight,mapDepth);

                    GameObject up = Instantiate(empty);
                    up.name = "Up";
                    up.tag = "Up";
                    up.transform.position = new Vector3(0, mapHeight/2, 0);
                    up.transform.parent = collider.transform;
                    BoxCollider boxColliderUp = up.AddComponent<BoxCollider>();
                    boxColliderUp.isTrigger = true;
                    boxColliderUp.size = new Vector3(mapWidth,.5f,mapDepth);

                    GameObject down = Instantiate(empty);
                    down.name = "Down";
                    down.tag = "Down";
                    down.transform.position = new Vector3(0, -mapHeight/2, 0);
                    down.transform.parent = collider.transform;
                    BoxCollider boxColliderDown = down.AddComponent<BoxCollider>();
                    boxColliderDown.isTrigger = true;
                    boxColliderDown.size = new Vector3(mapWidth,.5f,mapDepth);
                            
                    collider.transform.position=map.transform.position;
                }
            }
        }
    }   
}
```

Artık karakterimizi ışınlamaya başlayabiliriz.

## Karakterin ışınlanması
İlk olarak `Rigidbody`'si olan basit hareket scripti bulunan karakterimi `trigger`'landığında ışınlayacak bir kod denedim. Işınlanma düzgün çalışmadı ve ufak bir aramayla proje ayarlarından bir ayarın açılması gerektiğini öğrendim(Kırmızı ile işaretli):

![resim6](https://ergulburak.github.io/assets/img/sonsuzluk-2.PNG)

Artık düzgün çalışsada bir problem var! Collide olduğunda ışınlanıyor fakat ışınlandığı haritanin öteki ucunda da collider olduğu için tekrar ışınlanıyor. Böylece sonsuza yada ivmemizden fırlayana kadar ışınlanıyoruz. Bunu engellemek için karakterin çapı kadar içeriye ışınlıyorum. Bu olduğunda işe yarasada görüntüde atlama oluyor, yani biri sizi ileriye yarım saniyede itmiş gibi. Bunu çözmek için `triggerExit` kullanmayı seçiyorum. 

```csharp
private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Forward"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z-script.mapDepth+1
            );
        }
        if (other.CompareTag("Back"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z+script.mapDepth-1
            );
        }
        if (other.CompareTag("Right"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x-script.mapWidth+1,
                transform.position.y,
                transform.position.z
            );
        }
        if (other.CompareTag("Left"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x+script.mapWidth-1,
                transform.position.y,
                transform.position.z
            );
        }
        if (other.CompareTag("Up"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x,
                transform.position.y-script.mapHeight+1,
                transform.position.z
            );
        }
        if (other.CompareTag("Down"))
        {
            this.gameObject.transform.position = new Vector3(
                transform.position.x,
                transform.position.y+script.mapHeight-1,
                transform.position.z
            );
        }
    }
```

Bu sayede karakter triggerdan çıktığında zaten çapı kadar ilerlediği için yumuşak bir şekilde ışınlanmış oluyor.

[garden]: https://store.steampowered.com/app/473950/Manifold_Garden/
