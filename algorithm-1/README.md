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

On an 2 x 3 board, there are five tiles labeled from 1 to 5, and an empty square represented by 0. A move consists of choosing 0 and a 4-directionally adjacent number and swapping it.

The state of the board is solved if and only if the board is [[1,2,3],[4,5,0]].

Given the puzzle board board, return the least number of moves required so that the state of the board is solved. If it is impossible for the state of the board to be solved, return -1.

![image](https://user-images.githubusercontent.com/33912144/236757486-1de6785a-06a7-4d53-a7cb-3684a55b99b7.png)
Girdi: board = [[1,2,3],[4,0,5]]
Çıktı: 1
Açıklama: Bir hamlede 0 ve 5'i değiştirin.

![image](https://user-images.githubusercontent.com/33912144/236757638-1fe80d5a-d079-4c35-a2cb-0b31c292edf6.png)
Girdi: board = [[1,2,3],[5,4,0]]
Çıktı: -1
Açıklama: Hiçbir hamle sayısı tahtayı çözülmüş hale getirmeyecektir.

![image](https://user-images.githubusercontent.com/33912144/236757814-d4944172-e8a2-4e91-b46b-e0cbfd36d06f.png)
Girdi: board = [[4,1,2],[5,0,3]]
Çıktı: 5
Açıklama: 5, tahtayı çözen en küçük hamle sayısıdır.
Bir örnek yol:

1. hamleden sonra: [[4,1,2],[5,0,3]]
2. hamleden sonra: [[4,1,2],[0,5,3]]
3. hamleden sonra: [[0,1,2],[4,5,3]]
4. hamleden sonra: [[1,0,2],[4,5,3]]
5. hamleden sonra: [[1,2,0],[4,5,3]]
6. hamleden sonra: [[1,2,3],[4,5,0]]

Kısıtlamalar:

board.length == 2
board[i].length == 3
0 <= board[i][j] <= 5
Her board[i][j] değeri benzersizdir.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır - daha fazla bilgi için [LİSANS](LİSANS) dosyasına bakın.

