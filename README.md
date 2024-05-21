# Gem Collector - 2D Platform Game
## Mục lục
- [Gameplay](#Gameplay)
- [Cách chơi](#Cách-chơi)




## Gameplay
- Player sẽ là nhân vật màu tím.
- Trong game, player sẽ điều khiển nhân vật để thu thập các Gem.
- Gem có 2 loại: to, nhỏ.
- Mỗi khi nhặt mội loại Gem, thanh tiến trình của nhân vật sẽ tăng lên ứng với số điểm Gem đó mang lại (Gem to: 20 điểm, Gem nhỏ: 10 điểm)
- Trong game sẽ có các chướng ngại vật như:
    + Tường thẳng đứng (player bị chặn)
    + Mặt đất tự rơi khi player đứng trên
    + Bẫy gai màu đỏ (player dẫm vào sẽ trừ 1 máu)
    + Bẫy nhảy màu xanh lá (giúp player nhảy cao hơn nếu player nhảy vào)
    + Enemy màu đỏ sẽ luôn luôn đuổi theo player hoặc nhảy lên nếu đủ tầm để đuổi theo (trừ 1 máu player nếu đuổi kịp). Enemy sẽ xuất hiên ngẫu nhiên trên map.
    + Mặt đất di chuyển (mặt đất tự động di chuyển theo chu kì)
- Player cần vượt qua các chướng ngại vật cũng như tận dụng nó để thu thập các Gem, khi thanh tiến trình đầy thì có thể qua level mới
- Có 3 level, địa hình mỗi level sẽ khác nhau và độ khó tăng dần theo level.



## Cách chơi
### Cách điều khiển player
- Dùng phím A,D để di chuyển trái phải
- Dùng phím S để tuột xuống mặt đất (trừ mặt đất di chuyển và mặt đất dưới cùng thì đều có thể tuột xuống được)
- Phím Space để nhảy:
    + Nhấn 1 lần: nhảy thấp
    + Nhấn 1 lần và giữ: nhảy cao
    + Nhấn 2 lần: doubleJump giúp player nhảy 2 lần liên tục
    + Có thể kết hợp nhảy thấp hoặc cao vào nhấn 2 lần. Ví dụ: Nhấn 2 lần Space nhưng giữ 1 chút để nhảy cao nhất có thể
- Nhấn chuột trái để bắn ra đạn vào Enemy (mỗi đạn trừ 1hp của Enemy, Enemy có 3 máu)
- Dash: nhấn LShift để dash nhanh 1 khoảng
- Nếu player bị enemy bắt được (trừ 1 máu) hoặc dẫm bẫy gai (trừ 1 máu) tới hết máu (player có tối đa 3 máu) thì Game Over.
### Cách qua level
- Thu thập đủ Gem để đầy thanh tiến trình bên dưới
- Cách kiếm Gem:
    + Gem sẽ spawn ngẫu nhiên trên map. Khi đã xuất hiện, nếu player không nhặt thì Gem sẽ tự động biến mất sau 10 giây.
    + Giết Enemy để có thể nhặt Gem (tỉ lệ xuất hiện Gem sau khi Enemy bị giết là 60% với Gem to, 80% với Gem nhỏ)
    + Gem to: tăng 20% tiến trình, Gem nhỏ: tăng 10% tiến trình
    + Khi đầy tiến trình: Nhấn giữ R cho tới khi đầy vòng tròn để qua Level mới

### Các tính năng trong game:
- Âm thanh hiệu ứng: có các âm thanh hiệu ứng với các loại hoạt động như: nhặt item, nhảy vào bẫy nhảy, bắn đạn, bị enemy tấn công.
- Nhạc nền: có nhạc nền khi chơi game
- Tạm dừng: click pause (màu xanh lá) ở góc phải phía trên của màn hình game
    + Hiển thị màn hình tạm dừng game: có nút Continue(tiếp tục), Restart(chơi lại màn này) và Exit(thoát game)
- Menu scene: màn hình menu có nút Start(bắt đầu chơi) và Exit(thoát game)
- Khi player hết máu, hiển thị màn hình Game Over có nút Retry(thử lại) và dòng chữ thông báo đã qua được bao nhiêu LV. Nhấn Retry sẽ chơi lại từ đầu (LV1).
- Chỉnh âm lượng của nhạc nền riêng và âm lượng hiệu ứng riêng bằng 2 thanh slider (SFX và BGM), góc trên phải màn hình game, kéo để chỉnh âm lượng
- Khi thanh tiến trình của LV cuối đầy, nhấn giữ R cho tới khi đầy vòng tròn sẽ hiển thị màn hình chúc mừng hoàn thành game và nút Play Again (chơi lại từ đầu) và Exit(thoát game)
- Các Item và Enemy được xuất hiện ngẫu nhiên trên map, loại Item ngẫu nhiên sẽ được spawn ở vị trí ngẫu nhiên trên map, sẽ không có 2 Item và Enemy đứng cùng 1 vị trí
    + Có 3 loại Item: Gem to, Gem nhỏ, SpeedItem(tăng tốc độ nhân vật trong 1 khoảng thời gian)
    + Tối đa tổng Item và Enemy đang xuất hiện là 5
    + Item sẽ tự động biến mất sau 1 khoảng thời gian nếu Player không nhặt
    + Sau khi Item được nhặt hoặc tự động biến mất thì Item mới hoặc có thể Enemy sẽ tự động được tạo tiếp ở các vị trí ngẫu nhiên trên map để đảm bảo rằng tổng Item và Enemy đang xuất hiện luôn là 5.
    + Khả năng tối đa sẽ xuất hiện 5 Enemy
- Trong các map sẽ có các vùng bị ẩn (Hidden Area): khi player đến gần sẽ hiển thị, có thể khu đó sẽ có bẫy gai hoặc Item Heart(giúp tăng 1 máu cho Player)


### Chơi game để trải nghiệm sẽ thú vị hơn ạ
