## HATIRLATMA

Katkıda bulunmak istiyorsanız, lütfen aşağıdaki adımları takip edin:

1. Projeyi forklayın
2. Kendi dalınızı oluşturun (`git checkout -b ozellik/dal-adi`)
3. Çözmek istediğiniz algoritma dizinine geliniz.(algorithm-1,algorithm-2 vb.) Bu dizinde karşınıza programlama dilleri klasörleri çıkacaktır. Çözüm yapacağınız programlama dilinin dosya dizinine gelin. ( C#,Rust vb.) 
4. İlgili algoritma için çözümünüzü repo içerisinde kaç tane çözüm varsa ona göre isimlendirin. Eğer 2 adet çözüm varsa kendi çözümünüzü "solution-3.cs" şeklinde kaydedebilirsiniz.Bu numaralandırma şekliyle çözümleri daha derli toplu tutmayı hedefliyoruz. Farklı isimlendirmeler ile gönderilen PR 'lar otomatik olarak reddedilecektir.
6. Değişikliklerinizi commit edin (`git commit -am 'Değişiklikleri açıklayan mesaj'`)
7. Dalınıza push yapın (`git push origin ozellik/dal-adi`)
8. Bir Pull Request oluşturun


## SORU 

2 x 3'lük bir tahta üzerinde, 1'den 5'e kadar etiketlenmiş beş adet kutu ve 0 ile temsil edilen boş bir kare bulunmaktadır. Bir hamle, 0 ve 4 yönlü bitişik bir sayıyı seçip değiştirmekten oluşur.

Tahta durumu, tahta [[1,2,3],[4,5,0]] olduğunda çözülmüş sayılır.

Verilen tahta üzerindeki puzzle durumuna göre, tahta durumunun çözülmesi için gerekli en az hamle sayısını döndürün. Eğer tahta durumunun çözülmesi mümkün değilse, -1 döndürün.

![image](https://user-images.githubusercontent.com/33912144/236757486-1de6785a-06a7-4d53-a7cb-3684a55b99b7.png)

**Girdi:** board = [[1,2,3],[4,0,5]]  <br />
**Çıktı:** 1  <br />
**Açıklama:** Bir hamlede 0 ve 5'i değiştirin.

![image](https://user-images.githubusercontent.com/33912144/236757638-1fe80d5a-d079-4c35-a2cb-0b31c292edf6.png)

**Girdi:** board = [[1,2,3],[5,4,0]]  <br />
**Çıktı:** -1  <br />
**Açıklama:** Hiçbir hamle sayısı tahtayı çözülmüş hale getirmeyecektir.

![image](https://user-images.githubusercontent.com/33912144/236757814-d4944172-e8a2-4e91-b46b-e0cbfd36d06f.png)

**Girdi:** board = [[4,1,2],[5,0,3]]  <br />
**Çıktı:** 5  <br />
**Açıklama:** 5, tahtayı çözen en küçük hamle sayısıdır. <br />

**Bir örnek yol:**

1. hamleden sonra: [[4,1,2],[5,0,3]]
2. hamleden sonra: [[4,1,2],[0,5,3]]
3. hamleden sonra: [[0,1,2],[4,5,3]]
4. hamleden sonra: [[1,0,2],[4,5,3]]
5. hamleden sonra: [[1,2,0],[4,5,3]]
6. hamleden sonra: [[1,2,3],[4,5,0]]

**Kısıtlamalar:**

board.length == 2
board[i].length == 3
0 <= board[i][j] <= 5
Her board[i][j] değeri benzersizdir.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır - daha fazla bilgi için [LİSANS](LİSANS) dosyasına bakın.

