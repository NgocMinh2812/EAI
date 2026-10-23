using System;
using System.Collections.Generic;

namespace DocxGenerator
{
    public static class DataFile2Em
    {
        public static List<TestCase> GetSprint1TestCases()
        {
            var list = new List<TestCase>();

            // US-GV01 / T-05: Khung điều hướng Giảng viên
            list.Add(new TestCase {
                Id = "TC-01", Sprint = "Sprint 1", Module = "Điều hướng màn hình quản trị (US-GV01 / T-05)", TaskId = "US-GV01 / T-05",
                Scenario = "Chuyển đổi qua lại giữa các mục menu trên màn hình quản trị",
                Preconditions = "Đang đăng nhập bằng tài khoản Giảng viên.",
                TestData = "Nhấp chuột lần lượt: Tổng quan -> Quản lý người dùng -> Cây danh mục -> Soạn bài học",
                Steps = "1. Nhấp chọn lần lượt từng mục menu trên thanh bên trái màn hình.\n2. Quan sát giao diện tương ứng hiển thị ở giữa.\n3. Kiểm tra thanh chỉ dẫn đường dẫn trang ở phía trên.",
                Expected = "Màn hình chuyển đổi mượt mà, không bị giật hay đứng ứng dụng. Thanh chỉ dẫn hiển thị đúng tên mục đang mở (ví dụ: 'Trang chủ / Quản lý người dùng'). Nút menu đang chọn được làm nổi bật rõ ràng."
            });

            list.Add(new TestCase {
                Id = "TC-02", Sprint = "Sprint 1", Module = "Điều hướng màn hình quản trị (US-GV01 / T-05)", TaskId = "US-GV01 / T-05",
                Scenario = "Hiển thị thông tin cá nhân và tên viết tắt của giảng viên trên thanh trên cùng",
                Preconditions = "Đăng nhập tài khoản giảng viên 'Trần Thị Giảng Viên'.",
                TestData = "Họ tên: 'Trần Thị Giảng Viên', Vai trò: 'Giảng viên / Quản trị viên'",
                Steps = "1. Quan sát góc trên bên phải màn hình quản trị.\n2. Kiểm tra ảnh đại diện và tên người dùng hiển thị.",
                Expected = "Hiển thị chính xác họ tên đầy đủ 'Trần Thị Giảng Viên' kèm chức danh 'Giảng viên'. Ảnh đại diện hiển thị chữ viết tắt 'TG' rõ ràng, trang nhã."
            });

            list.Add(new TestCase {
                Id = "TC-03", Sprint = "Sprint 1", Module = "Điều hướng màn hình quản trị (US-GV01 / T-05)", TaskId = "US-GV01 / T-05",
                Scenario = "Đăng xuất tài khoản giảng viên an toàn khỏi hệ thống",
                Preconditions = "Đang ở giao diện quản trị của giảng viên.",
                TestData = "Bấm nút 'Đăng xuất' trên thanh menu bên trái",
                Steps = "1. Nhấn nút 'Đăng xuất'.\n2. Bấm 'Đồng ý' tại hộp thoại hỏi xác nhận đăng xuất.",
                Expected = "Màn hình quản trị đóng lại ngay lập tức, hệ thống xóa phiên làm việc hiện tại và quay về màn hình đăng nhập. Không thể bấm quay lại trang quản trị khi chưa đăng nhập lại."
            });

            list.Add(new TestCase {
                Id = "TC-04", Sprint = "Sprint 1", Module = "Điều hướng màn hình quản trị (US-GV01 / T-05)", TaskId = "US-GV01 / T-05",
                Scenario = "Phân quyền: Chặn học viên không được truy cập vào giao diện quản trị",
                Preconditions = "Có tài khoản học viên (sinhvien@englisheai.edu.vn).",
                TestData = "Tài khoản: sinhvien@englisheai.edu.vn, Mật khẩu: 123456",
                Steps = "1. Đăng nhập bằng tài khoản học viên.\n2. Kiểm tra xem hệ thống có mở giao diện quản trị hay không.",
                Expected = "Hệ thống nhận diện đúng vai trò học viên và chỉ mở màn hình học tập của học viên, tuyệt đối không cho phép truy cập vào khu vực quản trị của giảng viên."
            });

            // US-GV02 / T-11, T-12: Dashboard Tổng quan Hệ thống
            list.Add(new TestCase {
                Id = "TC-05", Sprint = "Sprint 1", Module = "Bảng thống kê tổng quan (US-GV02 / T-11, T-12)", TaskId = "US-GV02 / T-11, T-12",
                Scenario = "Hiển thị 4 thẻ thống kê tổng quan: Số học viên, Bài học, Câu hỏi và Lượt thi",
                Preconditions = "Hệ thống đang hoạt động và có dữ liệu thực tế.",
                TestData = "Mở trang Tổng quan trên màn hình giảng viên",
                Steps = "1. Mở xem trang Tổng quan của giảng viên.\n2. Đối chiếu số liệu hiển thị trên 4 thẻ thống kê với thực tế hệ thống.",
                Expected = "Cả 4 thẻ hiển thị chính xác số liệu thực tế gồm: Tổng số học viên, Tổng số bài học đã tạo, Số câu hỏi trong ngân hàng và Tổng số lượt thi đã thực hiện."
            });

            list.Add(new TestCase {
                Id = "TC-06", Sprint = "Sprint 1", Module = "Bảng thống kê tổng quan (US-GV02 / T-11, T-12)", TaskId = "US-GV02 / T-11, T-12",
                Scenario = "Tính tỷ lệ phần trăm tăng trưởng so với tuần trước và xử lý khi hệ thống mới",
                Preconditions = "Hệ thống có số liệu hoạt động của tuần này và tuần trước.",
                TestData = "Trường hợp 1: Có dữ liệu tăng trưởng; Trường hợp 2: Hệ thống mới tinh chưa có lượt thi nào",
                Steps = "1. Kiểm tra con số phần trăm tăng trưởng trên các thẻ thống kê.\n2. Kiểm tra trường hợp hệ thống mới chưa có dữ liệu cũ của tuần trước.",
                Expected = "Hiển thị đúng tỷ lệ phần trăm tăng hoặc giảm kèm mũi tên chỉ hướng. Khi hệ thống mới chưa có dữ liệu cũ, hiển thị số 0% an toàn không gây lỗi ứng dụng."
            });

            list.Add(new TestCase {
                Id = "TC-07", Sprint = "Sprint 1", Module = "Bảng thống kê tổng quan (US-GV02 / T-11, T-12)", TaskId = "US-GV02 / T-11, T-12",
                Scenario = "Hiển thị danh sách hoạt động học tập mới nhất của học viên",
                Preconditions = "Có học viên vừa hoàn thành bài học hoặc nộp bài thi.",
                TestData = "Danh sách hoạt động gần đây trên hệ thống",
                Steps = "1. Xem mục 'Hoạt động gần đây' trên trang Tổng quan.\n2. Đối chiếu tên học viên và thời gian vừa làm bài.",
                Expected = "Hiển thị danh sách các hoạt động mới nhất kèm tên học viên, việc vừa làm (ví dụ: 'Hoàn thành bài thi TOEIC đạt 8.0 điểm') và thời gian thực hiện (ví dụ: '5 phút trước')."
            });

            list.Add(new TestCase {
                Id = "TC-08", Sprint = "Sprint 1", Module = "Bảng thống kê tổng quan (US-GV02 / T-11, T-12)", TaskId = "US-GV02 / T-11, T-12",
                Scenario = "Biểu đồ xu hướng học tập trong tuần hiển thị trực quan và chính xác",
                Preconditions = "Hệ thống có dữ liệu học tập phân bổ theo các ngày trong tuần.",
                TestData = "Thống kê số lượt thi và giờ học 7 ngày gần nhất",
                Steps = "1. Quan sát biểu đồ học tập trên trang Tổng quan.\n2. Rê chuột vào các điểm mốc trên biểu đồ.",
                Expected = "Biểu đồ vẽ rõ ràng theo từng ngày từ Thứ 2 đến Chủ nhật. Rê chuột vào từng ngày hiển thị chi tiết số lượt học và số bài thi của ngày đó."
            });

            list.Add(new TestCase {
                Id = "TC-09", Sprint = "Sprint 1", Module = "Bảng thống kê tổng quan (US-GV02 / T-11, T-12)", TaskId = "US-GV02 / T-11, T-12",
                Scenario = "Nút 'Làm mới' cập nhật ngay số liệu mới nhất và xử lý khi mất mạng",
                Preconditions = "Có học viên vừa nộp thêm 1 bài thi ở bên ngoài.",
                TestData = "Bấm nút 'Làm mới' trên màn hình",
                Steps = "1. Bấm nút 'Làm mới' trên trang Tổng quan.\n2. Thử ngắt kết nối mạng rồi bấm nút Làm mới.",
                Expected = "Số lượt thi trên màn hình tự động tăng thêm 1 lượt ngay lập tức. Khi mất mạng, hiển thị thông báo 'Không thể cập nhật dữ liệu, vui lòng kiểm tra lại mạng' mà không làm mất số liệu đang hiển thị."
            });

            // US-GV03 / T-03, T-04, T-06: Quản lý Người dùng & Học viên
            list.Add(new TestCase {
                Id = "TC-10", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Hiển thị danh sách người dùng theo từng trang và chuyển trang qua lại",
                Preconditions = "Hệ thống có nhiều tài khoản người dùng, mỗi trang hiển thị 10 người.",
                TestData = "Chuyển trang: Trang 1, Trang 2, Trang cuối",
                Steps = "1. Mở trang Quản lý người dùng.\n2. Quan sát danh sách 10 người đầu tiên.\n3. Nhấn nút chuyển sang trang tiếp theo và trang cuối cùng.",
                Expected = "Danh sách hiển thị đúng 10 tài khoản trên một trang kèm họ tên, email, vai trò và trạng thái. Chuyển trang mượt mà, số trang hiển thị đúng (ví dụ: 'Trang 2 / 3')."
            });

            list.Add(new TestCase {
                Id = "TC-11", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Tìm kiếm người dùng nhanh theo Họ tên hoặc địa chỉ Email",
                Preconditions = "Đang ở trang Quản lý người dùng.",
                TestData = "Từ khóa tìm kiếm: 'Minh' hoặc 'sinhvien@'",
                Steps = "1. Nhập từ khóa vào ô tìm kiếm.\n2. Quan sát danh sách người dùng sau khi dừng gõ nửa giây.",
                Expected = "Danh sách tự động lọc ra đúng các tài khoản có họ tên hoặc email chứa từ khóa tìm kiếm. Số trang tự tính toán lại theo kết quả lọc."
            });

            list.Add(new TestCase {
                Id = "TC-12", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Lọc người dùng kết hợp theo Vai trò (Giảng viên, Sinh viên) và Trạng thái",
                Preconditions = "Hệ thống có đủ tài khoản giảng viên, học viên, tài khoản hoạt động và bị khóa.",
                TestData = "Chọn lọc: Vai trò là 'Sinh viên', Trạng thái là 'Đã khóa'",
                Steps = "1. Chọn ô lọc Vai trò là 'Sinh viên'.\n2. Chọn ô lọc Trạng thái là 'Đã khóa'.\n3. Quan sát danh sách hiển thị.",
                Expected = "Danh sách chỉ hiển thị đúng các sinh viên đang bị khóa tài khoản. Khi chọn lại 'Tất cả', danh sách hiển thị lại toàn bộ người dùng."
            });

            list.Add(new TestCase {
                Id = "TC-13", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Khóa và mở khóa tài khoản học viên trực tiếp trên danh sách",
                Preconditions = "Có tài khoản học viên đang ở trạng thái Hoạt động.",
                TestData = "Tài khoản học viên cần xử lý",
                Steps = "1. Nhấn nút 'Khóa tài khoản' tại dòng của học viên đó và xác nhận.\n2. Kiểm tra cột trạng thái.\n3. Nhấn nút 'Mở khóa' tại chính dòng tài khoản đó.",
                Expected = "Khi khóa: Cột trạng thái đổi sang nhãn 'Đã khóa', học viên đó không thể đăng nhập vào ứng dụng được nữa. Khi mở khóa: Trạng thái đổi lại thành 'Hoạt động', học viên đăng nhập lại bình thường."
            });

            list.Add(new TestCase {
                Id = "TC-14", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Chặn không cho giảng viên tự khóa chính tài khoản của mình",
                Preconditions = "Giảng viên đang đăng nhập tài khoản của chính mình.",
                TestData = "Dòng tài khoản của giảng viên đang sử dụng",
                Steps = "1. Tìm dòng tài khoản của chính mình trong danh sách người dùng.\n2. Quan sát nút thao tác Khóa.\n3. Thử bấm nút khóa tài khoản này.",
                Expected = "Nút Khóa tại dòng của chính giảng viên bị vô hiệu hóa (mờ đi) hoặc hệ thống cảnh báo: 'Bạn không thể tự khóa tài khoản của chính mình đang đăng nhập'."
            });

            list.Add(new TestCase {
                Id = "TC-15", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Cấp lại mật khẩu mặc định (123456) cho học viên khi quên mật khẩu",
                Preconditions = "Học viên quên mật khẩu và yêu cầu cấp lại.",
                TestData = "Tài khoản học viên cần cấp lại mật khẩu",
                Steps = "1. Chọn học viên trong danh sách và nhấn nút 'Cấp lại mật khẩu'.\n2. Xác nhận đồng ý.\n3. Thử đăng nhập bằng tài khoản học viên đó với mật khẩu 123456.",
                Expected = "Hệ thống đặt lại mật khẩu về mặc định '123456'. Học viên đăng nhập thành công với mật khẩu mới này."
            });

            list.Add(new TestCase {
                Id = "TC-16", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Kiểm tra các lỗi nhập liệu khi thêm tài khoản người dùng mới",
                Preconditions = "Bấm nút 'Thêm người dùng' để mở bảng nhập thông tin.",
                TestData = "Trường hợp 1: Để trống Họ tên hoặc Email;\nTrường hợp 2: Nhập email sai định dạng;\nTrường hợp 3: Nhập email đã có người đăng ký từ trước",
                Steps = "1. Lần lượt nhập các trường hợp sai sót trên vào bảng thêm người dùng.\n2. Nhấn nút 'Lưu'.\n3. Quan sát thông báo lỗi.",
                Expected = "Hệ thống ngăn chặn lưu ở mọi trường hợp không hợp lệ và hiển thị thông báo lỗi rõ ràng bên dưới ô nhập liệu tương ứng."
            });

            list.Add(new TestCase {
                Id = "TC-17", Sprint = "Sprint 1", Module = "Quản lý Người dùng & Học viên (US-GV03 / T-03, 04, 06)", TaskId = "US-GV03 / T-03, T-04, T-06",
                Scenario = "Khóa tài khoản có hiệu lực ngay lập tức khi học viên đang sử dụng",
                Preconditions = "Học viên đang mở ứng dụng sinh viên và học bài.",
                TestData = "Giảng viên bấm Khóa tài khoản của học viên đó",
                Steps = "1. Giảng viên khóa tài khoản học viên trên trang Quản lý người dùng.\n2. Phía học viên đang học bài, thử bấm chuyển bài hoặc nộp bài.",
                Expected = "Màn hình của học viên lập tức thông báo: 'Tài khoản của bạn đã bị khóa bởi quản trị viên'. Tự động đóng phiên làm việc và đẩy người dùng về màn hình đăng nhập."
            });

            // US-GV04 / T-07..T-10: Quản lý Danh mục & Cây Cấu trúc
            list.Add(new TestCase {
                Id = "TC-18", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Xem danh mục các cấp độ chuẩn (A1 đến C1) trong hệ thống",
                Preconditions = "Mở mục Quản lý danh mục.",
                TestData = "Danh mục cấp độ tiếng Anh",
                Steps = "1. Mở xem danh sách các cấp độ tiếng Anh.\n2. Quan sát các cấp độ hiển thị.",
                Expected = "Hiển thị đầy đủ 5 cấp độ chuẩn quốc tế: A1 (Cơ bản), A2 (Sơ cấp), B1 (Trung cấp), B2 (Trung cao cấp), C1 (Cao cấp) kèm mô tả chuẩn đầu ra."
            });

            list.Add(new TestCase {
                Id = "TC-19", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Hiển thị cây danh mục chủ đề học tập phân cấp cha và con",
                Preconditions = "Hệ thống có các chủ đề phân cấp nhiều tầng.",
                TestData = "Cây chủ đề: Tiếng Anh giao tiếp -> Công sở -> Phỏng vấn xin việc",
                Steps = "1. Mở xem cây chủ đề trên màn hình.\n2. Bấm vào các dấu mũi tên mở rộng và thu gọn nhánh thư mục.",
                Expected = "Cây danh mục hiển thị rõ ràng theo cấu trúc phân nhánh cha con, các chủ đề con thụt vào trong đúng nhánh, thao tác đóng mở nhánh mượt mà."
            });

            list.Add(new TestCase {
                Id = "TC-20", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Thêm một chủ đề con mới vào dưới một chủ đề cha đã chọn",
                Preconditions = "Đang chọn một thư mục cha (ví dụ: 'Ngữ pháp tiếng Anh').",
                TestData = "Tên chủ đề mới: 'Các thì trong tiếng Anh', Mô tả: 'Tổng hợp các thì cơ bản'",
                Steps = "1. Chọn thư mục cha.\n2. Bấm nút 'Thêm chủ đề con'.\n3. Nhập tên và mô tả rồi bấm 'Lưu'.",
                Expected = "Chủ đề mới được thêm thành công và xuất hiện ngay dưới nhánh của thư mục cha trên màn hình danh mục."
            });

            list.Add(new TestCase {
                Id = "TC-21", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Chỉnh sửa tên và mô tả của một chủ đề đã có sẵn",
                Preconditions = "Chọn một chủ đề trên cây danh mục.",
                TestData = "Đổi tên chủ đề và cập nhật mô tả mới",
                Steps = "1. Chọn chủ đề và nhấn nút 'Chỉnh sửa'.\n2. Thay đổi tên và mô tả trong bảng nhập liệu.\n3. Nhấn 'Lưu'.",
                Expected = "Tên chủ đề được cập nhật mới ngay lập tức trên cây danh mục."
            });

            list.Add(new TestCase {
                Id = "TC-22", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Di chuyển một chủ đề sang nằm dưới một thư mục cha khác",
                Preconditions = "Có chủ đề 'Viết Email' đang nằm ở mục 'Kỹ năng viết cơ bản'.",
                TestData = "Chuyển thư mục cha sang 'Tiếng Anh thương mại'",
                Steps = "1. Chọn chủ đề 'Viết Email' và bấm nút 'Di chuyển'.\n2. Chọn thư mục đích là 'Tiếng Anh thương mại'.\n3. Bấm 'Xác nhận chuyển'.",
                Expected = "Chủ đề 'Viết Email' được chuyển dời sang nằm dưới nhánh thư mục 'Tiếng Anh thương mại'."
            });

            list.Add(new TestCase {
                Id = "TC-23", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Ẩn hoặc hiện một chủ đề học tập đối với học viên",
                Preconditions = "Chọn một chủ đề đang hiển thị.",
                TestData = "Bật/tắt công tắc Trạng thái hoạt động",
                Steps = "1. Chọn chủ đề và gạt trạng thái sang 'Ẩn'.\n2. Kiểm tra bên giao diện của học viên.",
                Expected = "Bên học viên, chủ đề này cùng toàn bộ các bài học bên trong sẽ không hiển thị, giúp giảng viên yên tâm biên soạn nội dung chưa hoàn chỉnh."
            });

            list.Add(new TestCase {
                Id = "TC-24", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Ngăn chặn việc di chuyển thư mục cha vào làm con của chính nó",
                Preconditions = "Cây danh mục có thư mục Cha và thư mục Con.",
                TestData = "Cố ý chọn chuyển thư mục Cha vào làm con của thư mục Con của nó",
                Steps = "1. Chọn thư mục Cha.\n2. Bấm 'Di chuyển' và chọn thư mục đích là thư mục Con của nó.\n3. Nhấn 'Xác nhận'.",
                Expected = "Hệ thống phát hiện lỗi vòng lặp và cảnh báo: 'Không thể di chuyển thư mục cha vào làm thư mục con của chính nó'."
            });

            list.Add(new TestCase {
                Id = "TC-25", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Xóa chủ đề rỗng thành công và Chặn xóa chủ đề đang chứa bài học",
                Preconditions = "Thử 2 trường hợp: Chủ đề mới chưa có bài nào và Chủ đề đang chứa 10 bài học.",
                TestData = "Thử xóa 2 loại chủ đề trên",
                Steps = "1. Chọn chủ đề rỗng bấm 'Xóa' và xác nhận.\n2. Chọn chủ đề đang chứa bài học bấm 'Xóa'.",
                Expected = "Chủ đề rỗng được xóa sạch khỏi hệ thống. Chủ đề đang chứa bài học bị chặn xóa kèm thông báo: 'Không thể xóa chủ đề này vì đang có bài học/câu hỏi trực thuộc. Vui lòng di chuyển hoặc xóa các nội dung con trước'."
            });

            list.Add(new TestCase {
                Id = "TC-26", Sprint = "Sprint 1", Module = "Quản lý Cây Danh mục (US-GV04 / T-07..T-10)", TaskId = "US-GV04 / T-07..T-10",
                Scenario = "Cây danh mục hiển thị ổn định và chính xác sau khi khởi động lại ứng dụng",
                Preconditions = "Đã thực hiện thêm, sửa các chủ đề trên cây danh mục.",
                TestData = "Tắt ứng dụng và mở lại",
                Steps = "1. Tắt ứng dụng quản trị của giảng viên.\n2. Khởi chạy lại ứng dụng và mở lại cây danh mục.",
                Expected = "Toàn bộ cấu trúc cây danh mục và tên các chủ đề vẫn hiển thị chính xác như lúc vừa chỉnh sửa."
            });

            // T-13: Kiểm thử liên thông Sprint 1 WBS
            list.Add(new TestCase {
                Id = "TC-27", Sprint = "Sprint 1", Module = "Kiểm thử liên hoàn Sprint 1 (T-13)", TaskId = "T-13",
                Scenario = "Kiểm tra liên hoàn: Tạo chủ đề mới, tạo tài khoản học viên và xem trang Tổng quan",
                Preconditions = "Đang đăng nhập tài khoản Giảng viên.",
                TestData = "Tạo chủ đề 'Luyện thi TOEIC 2026' -> Thêm học viên 'Trần Văn Test'",
                Steps = "1. Vào mục Danh mục tạo chủ đề 'Luyện thi TOEIC 2026'.\n2. Vào mục Quản lý người dùng thêm học viên mới 'Trần Văn Test'.\n3. Chuyển sang trang Tổng quan kiểm tra.",
                Expected = "Trang Tổng quan tự động cập nhật số học viên tăng thêm 1 người, danh sách hoạt động ghi nhận tài khoản mới tạo. Luồng liên kết giữa các màn hình hoạt động trơn tru."
            });

            list.Add(new TestCase {
                Id = "TC-28", Sprint = "Sprint 1", Module = "Kiểm thử liên hoàn Sprint 1 (T-13)", TaskId = "T-13",
                Scenario = "Khóa tài khoản học viên rồi dùng tài khoản đó đăng nhập thử ở ngoài",
                Preconditions = "Học viên đang hoạt động bình thường.",
                TestData = "Tài khoản học viên cần thử nghiệm",
                Steps = "1. Giảng viên vào Quản lý người dùng bấm Khóa tài khoản này.\n2. Mở màn hình đăng nhập bên ngoài, nhập email và mật khẩu của tài khoản vừa bị khóa và bấm Đăng nhập.",
                Expected = "Đăng nhập bị từ chối kèm thông báo: 'Tài khoản của bạn đã bị khóa hoặc tạm dừng hoạt động'. Xác nhận tính đồng bộ giữa phân hệ Quản trị và phân hệ Đăng nhập."
            });

            return list;
        }

        public static List<TestCase> GetSprint2TestCases()
        {
            var list = new List<TestCase>();

            // US-GV05 / T-14..T-17: Quản lý Nội dung Bài học
            list.Add(new TestCase {
                Id = "TC-29", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Tạo bài học mới theo chủ đề và trình độ với đầy đủ thông tin hợp lệ",
                Preconditions = "Mở trang Quản lý bài học, nhấn 'Tạo bài học mới'.",
                TestData = "Tiêu đề: 'Thì quá khứ tiếp diễn', Chủ đề: 'Ngữ pháp tiếng Anh', Trình độ: 'B1', Thời lượng: 20 phút, Kỹ năng: 'Ngữ pháp'",
                Steps = "1. Nhập tiêu đề, chọn chủ đề, trình độ B1, kỹ năng Ngữ pháp.\n2. Nhập nội dung bài giảng chi tiết vào khung soạn thảo.\n3. Nhấn nút 'Lưu bài học'.",
                Expected = "Bài học được tạo thành công và xuất hiện ngay trên danh sách bài học của giảng viên ở trạng thái 'Bản nháp' (chưa xuất bản)."
            });

            list.Add(new TestCase {
                Id = "TC-30", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Kiểm tra bắt buộc nhập tiêu đề và thời lượng hợp lệ khi tạo bài học",
                Preconditions = "Đang ở form soạn bài học.",
                TestData = "Trường hợp 1: Để trống Tiêu đề; Trường hợp 2: Nhập thời lượng bằng 0 hoặc số âm",
                Steps = "1. Để trống ô tiêu đề rồi bấm Lưu.\n2. Nhập tiêu đề nhưng để thời lượng bằng -5 rồi bấm Lưu.",
                Expected = "Hệ thống chặn lại và thông báo lỗi rõ ràng: 'Vui lòng nhập tiêu đề bài học' và 'Thời lượng bài học phải lớn hơn 0'. Không có dữ liệu rác nào được lưu vào hệ thống."
            });

            list.Add(new TestCase {
                Id = "TC-31", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Phân loại bài học theo 5 kỹ năng chuẩn: Từ vựng, Ngữ pháp, Đọc, Nghe, Viết",
                Preconditions = "Form soạn bài học có danh sách chọn Kỹ năng.",
                TestData = "5 kỹ năng cốt lõi",
                Steps = "1. Nhấp mở ô chọn 'Kỹ năng'.\n2. Kiểm tra danh sách các kỹ năng có thể chọn.",
                Expected = "Hiển thị đầy đủ 5 kỹ năng: Từ vựng, Ngữ pháp, Đọc hiểu, Nghe hiểu, Viết. Chọn kỹ năng nào thì bài học lưu đúng kỹ năng đó."
            });

            list.Add(new TestCase {
                Id = "TC-32", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Đính kèm file âm thanh nghe thử và ảnh minh họa xem trước cho bài học",
                Preconditions = "Có sẵn file âm thanh mẫu (.mp3) và file ảnh mẫu (.png).",
                TestData = "File âm thanh .mp3 và file ảnh .png",
                Steps = "1. Bấm nút chọn file âm thanh và bấm 'Nghe thử' tại trình phát.\n2. Bấm nút chọn ảnh minh họa và quan sát khung xem trước.",
                Expected = "File âm thanh phát rõ ràng trên trình nghe thử; ảnh minh họa hiển thị sắc nét trong khung xem trước kèm thông tin dung lượng tệp."
            });

            list.Add(new TestCase {
                Id = "TC-33", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Ngăn chặn việc chọn nhầm file sai định dạng khi đính kèm bài học",
                Preconditions = "Có file văn bản hoặc file cài đặt (.exe, .txt).",
                TestData = "Thử chọn file .exe hoặc .txt vào ô đính kèm âm thanh",
                Steps = "1. Cố ý chọn file .exe hoặc .txt vào ô đính kèm âm thanh hoặc ảnh.\n2. Quan sát cảnh báo của hệ thống.",
                Expected = "Hệ thống tự lọc định dạng chỉ cho phép chọn đuôi file nhạc (.mp3, .wav) và file ảnh (.png, .jpg), báo lỗi nếu cố tình chọn sai."
            });

            list.Add(new TestCase {
                Id = "TC-34", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Bật hoặc tắt xuất bản bài học và kiểm tra sự xuất hiện bên phía học viên",
                Preconditions = "Bài học mới soạn xong đang ở trạng thái Bản nháp.",
                TestData = "Bật/tắt công tắc 'Xuất bản bài học'",
                Steps = "1. Bật công tắc 'Xuất bản bài học' và bấm Lưu. Mở ứng dụng sinh viên kiểm tra.\n2. Tắt công tắc xuất bản và Lưu lại. Kiểm tra lại bên học viên.",
                Expected = "Khi bật xuất bản: Học viên nhìn thấy bài học và vào học bình thường. Khi tắt xuất bản: Bài học biến mất ngay khỏi danh sách của học viên, đảm bảo nội dung chưa hoàn chỉnh không bị lộ."
            });

            list.Add(new TestCase {
                Id = "TC-35", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Chỉnh sửa bài học mà không làm mất file âm thanh và ảnh đã đính kèm",
                Preconditions = "Bài học đã có sẵn file âm thanh và ảnh đính kèm từ trước.",
                TestData = "Sửa một đoạn văn bản trong bài giảng mà không chọn lại file đính kèm",
                Steps = "1. Mở bài học để chỉnh sửa một đoạn văn bản lý thuyết.\n2. Nhấn nút 'Cập nhật bài học'.",
                Expected = "Đoạn văn bản được cập nhật mới, các file âm thanh và ảnh đính kèm từ trước vẫn được giữ nguyên vẹn."
            });

            list.Add(new TestCase {
                Id = "TC-36", Sprint = "Sprint 2", Module = "Quản lý nội dung bài học (US-GV05 / T-14..17)", TaskId = "US-GV05 / T-14..T-17",
                Scenario = "Xử lý lưu trữ an toàn khi gặp sự cố ngắt kết nối lúc lưu bài học",
                Preconditions = "Giả lập lỗi ngắt kết nối mạng khi đang nhấn Lưu.",
                TestData = "Ngắt kết nối mạng đúng thời điểm bấm Lưu bài học",
                Steps = "1. Nhập nội dung bài học hoàn chỉnh.\n2. Ngắt mạng rồi nhấn nút Lưu.\n3. Quan sát thông báo lỗi.",
                Expected = "Hệ thống báo lỗi kết nối và giữ nguyên toàn bộ nội dung bài giảng trên form, không làm người dùng bị mất công soạn thảo."
            });

            // US-GV06 / T-18..T-20: Quản lý Ngân hàng Câu hỏi
            list.Add(new TestCase {
                Id = "TC-37", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Tạo câu hỏi trắc nghiệm 4 lựa chọn (A, B, C, D) với 1 đáp án đúng",
                Preconditions = "Mở trang Ngân hàng câu hỏi, nhấn 'Thêm câu hỏi'.",
                TestData = "Câu hỏi: 'She ___ to the cinema yesterday.', 4 đáp án: A. go, B. went (Đúng), C. gone, D. goes",
                Steps = "1. Nhập nội dung câu hỏi trắc nghiệm.\n2. Nhập nội dung cho 4 đáp án A, B, C, D.\n3. Tích chọn ô tròn tại đáp án B đánh dấu là đáp án đúng.\n4. Nhấn 'Lưu câu hỏi'.",
                Expected = "Câu hỏi được lưu thành công vào ngân hàng kèm 4 phương án, đáp án B được ghi nhận là đáp án đúng, câu hỏi xuất hiện trên danh sách ngân hàng."
            });

            list.Add(new TestCase {
                Id = "TC-38", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Tạo câu hỏi dạng điền từ vào chỗ trống và kiểm tra cách hiển thị",
                Preconditions = "Chọn loại câu hỏi 'Điền từ vào chỗ trống'.",
                TestData = "Nội dung: 'I have lived in Hanoi [since] 2010.', đáp án đúng là 'since'",
                Steps = "1. Chọn loại câu hỏi điền từ.\n2. Nhập câu hỏi có từ khóa trong ngoặc vuông [since].\n3. Nhấn 'Lưu'.",
                Expected = "Hệ thống tự động nhận diện từ khóa và tạo ra chỗ trống [...] để học viên điền từ lúc làm bài thi."
            });

            list.Add(new TestCase {
                Id = "TC-39", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Tạo câu hỏi đọc hiểu đính kèm đoạn văn bản dài",
                Preconditions = "Chọn loại câu hỏi 'Đọc hiểu'.",
                TestData = "Đoạn văn đọc hiểu dài 300 từ về chủ đề Môi trường",
                Steps = "1. Nhập đoạn văn mẫu vào ô 'Đoạn văn đọc hiểu'.\n2. Nhập câu hỏi liên quan và 4 phương án trả lời.\n3. Nhấn 'Lưu'.",
                Expected = "Đoạn văn đọc hiểu được lưu đầy đủ. Khi hiển thị cho thí sinh sẽ chia đôi màn hình: bên trái là đoạn văn có thanh cuộn riêng, bên phải là câu hỏi và các phương án."
            });

            list.Add(new TestCase {
                Id = "TC-40", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Tạo câu hỏi nghe đính kèm file ghi âm và nghe thử âm thanh",
                Preconditions = "Chọn loại câu hỏi 'Nghe hiểu'.",
                TestData = "File âm thanh bài nghe",
                Steps = "1. Nhấn nút đính kèm file âm thanh cho câu hỏi.\n2. Bấm 'Nghe thử' tại trình phát nhỏ bên cạnh.\n3. Nhập câu hỏi và các đáp án rồi bấm Lưu.",
                Expected = "File âm thanh liên kết đúng với câu hỏi. Khi học viên làm bài thi, trình phát âm thanh nhỏ sẽ xuất hiện ngay phía trên câu hỏi cho phép nghe đoạn hội thoại."
            });

            list.Add(new TestCase {
                Id = "TC-41", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Nhập lời giải thích chi tiết đáp án và phân tích ngữ pháp cho câu hỏi",
                Preconditions = "Đang soạn câu hỏi.",
                TestData = "Giải thích: 'Dấu hiệu 'yesterday' chỉ quá khứ đơn, nên động từ 'go' chia thành 'went'.'",
                Steps = "1. Nhập nội dung phân tích vào ô 'Giải thích chi tiết'.\n2. Nhấn Lưu câu hỏi.",
                Expected = "Lời giải thích được lưu vào hệ thống và sẽ hiển thị cho học viên xem lại sau khi nộp bài thi để ôn tập."
            });

            list.Add(new TestCase {
                Id = "TC-42", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Gắn mức độ khó (Dễ, Trung bình, Khó) và đánh dấu Đã kiểm duyệt",
                Preconditions = "Form thêm câu hỏi có ô chọn Độ khó và ô Đã kiểm duyệt.",
                TestData = "Độ khó: 'Khó', tích chọn Đã kiểm duyệt",
                Steps = "1. Chọn độ khó 'Khó'.\n2. Đánh dấu vào ô 'Đã kiểm duyệt' (Sẵn sàng đưa vào đề thi).\n3. Nhấn Lưu.",
                Expected = "Câu hỏi lưu đúng độ khó 'Khó' và trạng thái đã duyệt, đủ điều kiện để đưa vào các đề thi thử chính thức."
            });

            list.Add(new TestCase {
                Id = "TC-43", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Kiểm tra bắt buộc có từ 2 đáp án trở lên và phải chọn 1 đáp án đúng",
                Preconditions = "Đang ở form tạo câu hỏi trắc nghiệm.",
                TestData = "Trường hợp 1: Chỉ nhập 1 đáp án; Trường hợp 2: Nhập 4 đáp án nhưng không chọn đáp án đúng nào",
                Steps = "1. Thử nghiệm lần lượt các trường hợp vi phạm trên form.\n2. Nhấn nút 'Lưu câu hỏi' ở từng trường hợp.",
                Expected = "Hệ thống chặn lại và thông báo: 'Câu hỏi trắc nghiệm phải có ít nhất 2 đáp án lựa chọn' và 'Vui lòng chọn 1 đáp án đúng cho câu hỏi'."
            });

            list.Add(new TestCase {
                Id = "TC-44", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Đảm bảo lưu câu hỏi và toàn bộ các đáp án đồng thời, không bị sót",
                Preconditions = "Thực hiện lưu câu hỏi trắc nghiệm có 4 đáp án.",
                TestData = "1 câu hỏi kèm 4 lựa chọn đáp án",
                Steps = "1. Nhấn nút 'Lưu câu hỏi'.\n2. Kiểm tra danh sách câu hỏi trong ngân hàng.",
                Expected = "Câu hỏi và đủ cả 4 phương án được lưu hoàn chỉnh cùng lúc, không bao giờ xảy ra tình trạng câu hỏi bị thiếu mất đáp án."
            });

            list.Add(new TestCase {
                Id = "TC-45", Sprint = "Sprint 2", Module = "Ngân hàng Câu hỏi (US-GV06 / T-18..T-20)", TaskId = "US-GV06 / T-18..T-20",
                Scenario = "Tìm kiếm câu hỏi theo từ khóa và chỉnh sửa nội dung câu hỏi",
                Preconditions = "Ngân hàng có nhiều câu hỏi.",
                TestData = "Tìm từ khóa 'cinema', lọc kỹ năng Ngữ pháp, mở câu hỏi để sửa",
                Steps = "1. Nhập từ khóa 'cinema' vào thanh tìm kiếm, chọn lọc kỹ năng Ngữ pháp.\n2. Chọn câu hỏi tìm thấy và nhấn nút 'Chỉnh sửa'.\n3. Sửa lại nội dung và nhấn Lưu.",
                Expected = "Tìm đúng câu hỏi cần tìm. Bảng sửa câu hỏi tải đủ nội dung cũ và các phương án. Cập nhật thành công mà không bị nhân bản thêm câu hỏi trùng lặp."
            });

            // US-GV07 / T-21, T-22: Quản lý Đề thi & Gói Luyện tập
            list.Add(new TestCase {
                Id = "TC-46", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Tạo đề thi thử chính thức có cài đặt thời gian làm bài đếm ngược",
                Preconditions = "Mở mục Quản lý đề thi, nhấn 'Tạo đề thi mới'.",
                TestData = "Tên đề thi: 'Đề thi thử TOEIC Đợt 1', Thời gian: 60 phút, Thể loại: 'Thi thử (Có tính giờ)', Trình độ: 'B2'",
                Steps = "1. Nhập tên đề thi, chọn loại có tính giờ, nhập thời gian 60 phút.\n2. Nhấn 'Lưu thông tin đề thi'.",
                Expected = "Đề thi được tạo thành công ở trạng thái 'Bản nháp' (Chưa phát hành) để giảng viên chọn câu hỏi đưa vào đề thi."
            });

            list.Add(new TestCase {
                Id = "TC-47", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Tạo gói bài tập luyện tập tự do không áp lực thời gian",
                Preconditions = "Chọn thể loại 'Gói Luyện tập tự do'.",
                TestData = "Tên gói: 'Bộ bài tập ngữ pháp A1', Thời gian: 0 phút",
                Steps = "1. Chọn thể loại Luyện tập tự do, để thời gian = 0 phút.\n2. Nhấn 'Lưu gói luyện tập'.",
                Expected = "Gói luyện tập được tạo thành công. Khi học viên làm bài gói này sẽ không có đồng hồ đếm ngược, được tự do suy nghĩ làm bài."
            });

            list.Add(new TestCase {
                Id = "TC-48", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Chọn câu hỏi từ ngân hàng đưa vào đề thi và sắp xếp thứ tự, điểm số",
                Preconditions = "Đang ở màn hình chọn câu hỏi cho đề thi.",
                TestData = "Chọn 10 câu hỏi từ ngân hàng; Cấu hình mỗi câu 1.0 điểm; Đổi vị trí câu 1 xuống câu 5",
                Steps = "1. Đánh dấu chọn 10 câu hỏi trong ngân hàng và bấm 'Thêm vào đề thi'.\n2. Nhập điểm 1.0 cho từng câu.\n3. Dùng nút mũi tên di chuyển lên/xuống để đổi thứ tự câu hỏi.\n4. Nhấn 'Lưu cấu hình câu hỏi'.",
                Expected = "Đề thi có đủ 10 câu hỏi với thứ tự và điểm số đúng như vừa sắp xếp."
            });

            list.Add(new TestCase {
                Id = "TC-49", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Cài đặt điểm đạt yêu cầu và bật tính năng xáo trộn thứ tự câu hỏi",
                Preconditions = "Đề thi có 10 câu hỏi, tổng điểm 10.0.",
                TestData = "Điểm đạt: 6.0 điểm, Bật tính năng xáo trộn câu hỏi",
                Steps = "1. Nhập Điểm đạt yêu cầu: 6.0 điểm.\n2. Bật công tắc 'Xáo trộn thứ tự câu hỏi khi thi'.\n3. Nhấn Lưu cấu hình.",
                Expected = "Khi học viên làm bài thi, thứ tự câu hỏi sẽ được đảo ngẫu nhiên để chống nhìn bài của nhau, học viên đạt từ 6.0 điểm trở lên là đạt."
            });

            list.Add(new TestCase {
                Id = "TC-50", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Kiểm tra bắt buộc đề thi phải có câu hỏi trước khi phát hành",
                Preconditions = "Thử phát hành một đề thi chưa có câu hỏi nào.",
                TestData = "Đề thi có 0 câu hỏi",
                Steps = "1. Nhấn nút 'Phát hành đề thi' khi đề thi chưa được gán bất kỳ câu hỏi nào.\n2. Quan sát phản hồi.",
                Expected = "Hệ thống chặn lại và cảnh báo: 'Đề thi phải có ít nhất 1 câu hỏi trước khi phát hành'. Không cho phép phát hành đề thi rỗng."
            });

            list.Add(new TestCase {
                Id = "TC-51", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Phát hành đề thi và kiểm tra sự xuất hiện bên phía học viên",
                Preconditions = "Đề thi đã hoàn thiện đầy đủ câu hỏi, thời gian và điểm số.",
                TestData = "Nhấn nút 'Phát hành đề thi'",
                Steps = "1. Nhấn nút 'Phát hành đề thi'.\n2. Xác nhận đồng ý tại hộp thoại phê duyệt.\n3. Mở bên ứng dụng học viên kiểm tra.",
                Expected = "Đề thi chuyển sang trạng thái 'Đã phát hành' và xuất hiện ngay trên danh sách đề thi thử của học viên."
            });

            list.Add(new TestCase {
                Id = "TC-52", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Đề thi đã phát hành không bị ảnh hưởng khi ngân hàng câu hỏi bị sửa",
                Preconditions = "Đề thi đã phát hành có chứa câu hỏi số 10; giảng viên vào ngân hàng câu hỏi sửa câu số 10.",
                TestData = "Sửa câu hỏi số 10 trong ngân hàng",
                Steps = "1. Giảng viên vào Ngân hàng câu hỏi sửa nội dung câu hỏi số 10.\n2. Mở đề thi đã phát hành ra xem lại.",
                Expected = "Đề thi đã phát hành vẫn giữ nguyên nội dung gốc để đảm bảo tính công bằng và ổn định của kỳ thi."
            });

            list.Add(new TestCase {
                Id = "TC-53", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Xem trước giao diện phòng thi giả lập trước khi phát hành đề thi",
                Preconditions = "Chọn một đề thi đã tạo.",
                TestData = "Bấm nút 'Xem trước đề thi'",
                Steps = "1. Nhấn nút 'Xem trước đề thi'.\n2. Duyệt qua giao diện phòng thi giả lập.",
                Expected = "Mở ra giao diện mô phỏng phòng thi giống của học viên: có đồng hồ đếm ngược, danh sách câu hỏi, bảng số câu và các phương án trả lời để giảng viên rà soát lần cuối."
            });

            list.Add(new TestCase {
                Id = "TC-54", Sprint = "Sprint 2", Module = "Quản lý Đề thi & Luyện tập (US-GV07 / T-21, 22)", TaskId = "US-GV07 / T-21, T-22",
                Scenario = "Thông tin đề thi lưu trữ ổn định khi tắt và mở lại ứng dụng",
                Preconditions = "Đã tạo và cấu hình đề thi hoàn chỉnh.",
                TestData = "Tắt ứng dụng và mở lại",
                Steps = "1. Kiểm tra danh sách đề thi sau khi khởi động lại ứng dụng.\n2. Mở xem chi tiết các câu hỏi trong đề thi.",
                Expected = "Toàn bộ thông tin đề thi, thời lượng, điểm đạt và danh sách câu hỏi trực thuộc vẫn được lưu trữ bền vững và đầy đủ."
            });

            // T-23: Kiểm thử liên thông Sprint 2 WBS
            list.Add(new TestCase {
                Id = "TC-55", Sprint = "Sprint 2", Module = "Kiểm thử liên hoàn Sprint 2 (T-23)", TaskId = "T-23",
                Scenario = "Kiểm tra liên hoàn: Giảng viên soạn bài, xuất bản và học viên vào học",
                Preconditions = "Mở ứng dụng giảng viên và ứng dụng học viên.",
                TestData = "Soạn bài học nghe mới có file âm thanh",
                Steps = "1. Giảng viên soạn bài học nghe mới, đính kèm file âm thanh.\n2. Bật xuất bản và Lưu.\n3. Đăng nhập ứng dụng học viên bằng tài khoản sinh viên.\n4. Vào mục Lộ trình học kiểm tra và bấm nghe thử âm thanh bài học.",
                Expected = "Học viên nhìn thấy ngay bài học mới trong danh mục, bấm vào học và nghe âm thanh rõ ràng, xác nhận tính liên thông hoàn hảo giữa quản trị và học viên."
            });

            list.Add(new TestCase {
                Id = "TC-56", Sprint = "Sprint 2", Module = "Kiểm thử liên hoàn Sprint 2 (T-23)", TaskId = "T-23",
                Scenario = "Kiểm tra liên hoàn: Giảng viên phát hành đề, học viên thi và xem số liệu tự cập nhật",
                Preconditions = "Đang ở ứng dụng giảng viên.",
                TestData = "Tạo 5 câu hỏi mới -> Gom vào đề thi 15 phút -> Phát hành -> Học viên thi được 8.0đ",
                Steps = "1. Giảng viên tạo 5 câu hỏi trong Ngân hàng câu hỏi.\n2. Tạo đề thi 'Kiểm tra 15 phút', gán 5 câu hỏi vừa tạo và bấm Phát hành.\n3. Học viên đăng nhập vào Thi thử, làm bài và nộp bài đạt 8.0 điểm.\n4. Giảng viên quay lại trang Tổng quan kiểm tra số liệu.",
                Expected = "Chu trình khép kín vận hành hoàn hảo: Đề thi phát hành thành công -> Học viên thi và được chấm điểm tự động -> Số lượt thi trên trang Tổng quan của Giảng viên tự động tăng thêm 1 lượt."
            });

            return list;
        }

        public static List<TestCase> GetE2ETestCases()
        {
            var list = new List<TestCase>();

            // E2E Toàn trình
            list.Add(new TestCase {
                Id = "TC-57", Sprint = "E2E", Module = "Kiểm thử toàn trình (End-to-End)", TaskId = "US-GV04, US-GV05, US-GV06, US-GV07",
                Scenario = "Kiểm tra toàn bộ chu trình quản lý: Tạo danh mục, soạn bài học, tạo câu hỏi và phát hành đề thi",
                Preconditions = "Giảng viên đăng nhập vào hệ thống quản trị.",
                TestData = "Tạo chủ đề mới -> Soạn bài học -> Tạo câu hỏi -> Phát hành đề thi thử",
                Steps = "1. Vào mục Danh mục tạo chủ đề mới 'Luyện thi TOEIC 2026'.\n2. Vào mục Bài học soạn bài học thuộc chủ đề này và xuất bản.\n3. Vào Ngân hàng câu hỏi tạo các câu hỏi trắc nghiệm.\n4. Vào Quản lý đề thi tạo đề thi thử, gán câu hỏi và phát hành đề thi.\n5. Đăng xuất an toàn.",
                Expected = "Toàn bộ chu trình quản lý vận hành trơn tru: Cây danh mục cập nhật nhánh mới, bài học hiển thị đúng chủ đề, ngân hàng câu hỏi đầy đủ và đề thi phát hành sẵn sàng cho học viên. Dữ liệu đồng bộ chính xác."
            });

            list.Add(new TestCase {
                Id = "TC-58", Sprint = "E2E", Module = "Kiểm thử toàn trình (End-to-End)", TaskId = "US-GV01, US-GV03",
                Scenario = "Kiểm tra xử lý học viên vi phạm: Khóa tài khoản, cấp lại mật khẩu và mở khóa",
                Preconditions = "Tài khoản giảng viên đang đăng nhập.",
                TestData = "Khóa tài khoản vi phạm -> Thử đăng nhập ngoài -> Cấp lại mật khẩu -> Mở khóa",
                Steps = "1. Mở Quản lý người dùng, tìm kiếm học viên theo email.\n2. Bấm nút khóa tài khoản học viên đó.\n3. Ra màn hình đăng nhập bên ngoài xác nhận tài khoản bị chặn không vào được.\n4. Giảng viên cấp lại mật khẩu về 123456 và mở khóa tài khoản.\n5. Đăng nhập lại để xác nhận tài khoản hoạt động bình thường.",
                Expected = "Cơ chế quản lý tài khoản và bảo mật hoạt động an toàn tuyệt đối, cập nhật trạng thái có hiệu lực ngay lập tức trên hệ thống."
            });

            return list;
        }

        public static List<TraceItem> GetTraceabilityItems()
        {
            var list = new List<TraceItem>();

            list.Add(new TraceItem { Code = "US-GV01 / T-05", Name = "Khung điều hướng Giảng viên", AcceptanceCriteria = "Chuyển trang mượt mà, thanh chỉ dẫn đường dẫn, làm nổi bật menu đang chọn, avatar, đăng xuất an toàn, phân quyền", TestCaseIds = "TC-01, TC-02, TC-03, TC-04", Note = "Gộp điều hướng và highlight vào TC-01" });
            list.Add(new TraceItem { Code = "US-GV02 / T-11, T-12", Name = "Bảng thống kê tổng quan (Dashboard)", AcceptanceCriteria = "4 thẻ thống kê thực tế, tính % tăng trưởng, hiển thị số 0 an toàn, hoạt động gần đây, biểu đồ xu hướng, nút Làm mới", TestCaseIds = "TC-05, TC-06, TC-07, TC-08, TC-09", Note = "Gộp tính tăng trưởng vào TC-06, nút làm mới vào TC-09" });
            list.Add(new TraceItem { Code = "US-GV03 / T-03, 04, 06", Name = "Quản lý Người dùng & Học viên", AcceptanceCriteria = "Phân trang 10 người/trang, tìm kiếm nhanh, lọc vai trò/trạng thái, khóa/mở khóa, chặn tự khóa mình, cấp lại mật khẩu 123456", TestCaseIds = "TC-10, TC-11, TC-12, TC-13, TC-14, TC-15, TC-16, TC-17", Note = "Gộp phân trang vào TC-10, lọc kết hợp vào TC-12, khóa/mở khóa vào TC-13" });
            list.Add(new TraceItem { Code = "US-GV04 / T-07..10", Name = "Quản lý Cây Danh mục chủ đề", AcceptanceCriteria = "Danh mục cấp độ A1-C1, cây chủ đề phân cấp cha con, thêm/sửa/di chuyển/ẩn chủ đề, chặn lỗi lặp vô tận, chặn xóa khi có bài học", TestCaseIds = "TC-18, TC-19, TC-20, TC-21, TC-22, TC-23, TC-24, TC-25, TC-26", Note = "Gộp xóa rỗng và chặn xóa vào TC-25" });
            list.Add(new TraceItem { Code = "T-13", Name = "Kiểm thử liên hoàn Sprint 1 WBS", AcceptanceCriteria = "Tạo chủ đề -> Thêm học viên -> Xem Dashboard; Khóa tài khoản và kiểm tra chặn đăng nhập ngoài", TestCaseIds = "TC-27, TC-28", Note = "Kiểm tra liên hoàn Sprint 1" });

            list.Add(new TraceItem { Code = "US-GV05 / T-14..17", Name = "Quản lý Nội dung Bài học", AcceptanceCriteria = "Soạn bài học theo chủ đề/trình độ, kiểm tra bắt buộc nhập liệu, 5 kỹ năng, đính kèm âm thanh/ảnh, xuất bản, xử lý lưu an toàn", TestCaseIds = "TC-29, TC-30, TC-31, TC-32, TC-33, TC-34, TC-35, TC-36", Note = "Gộp đính kèm âm thanh và ảnh vào TC-32, bật tắt xuất bản vào TC-34" });
            list.Add(new TraceItem { Code = "US-GV06 / T-18..20", Name = "Quản lý Ngân hàng Câu hỏi", AcceptanceCriteria = "Thêm trắc nghiệm 4 lựa chọn, điền từ, đọc hiểu đoạn văn, câu hỏi nghe, giải thích chi tiết, độ khó/kiểm duyệt, bắt lỗi nhập", TestCaseIds = "TC-37, TC-38, TC-39, TC-40, TC-41, TC-42, TC-43, TC-44, TC-45", Note = "Gộp bắt lỗi lựa chọn vào TC-43, tìm kiếm và sửa câu hỏi vào TC-45" });
            list.Add(new TraceItem { Code = "US-GV07 / T-21, 22", Name = "Quản lý Đề thi & Gói Luyện tập", AcceptanceCriteria = "Tạo đề thi tính giờ, bài tập tự do, chọn câu hỏi từ ngân hàng, sắp xếp thứ tự điểm số, điểm đạt/xáo trộn, phát hành, xem trước phòng thi", TestCaseIds = "TC-46, TC-47, TC-48, TC-49, TC-50, TC-51, TC-52, TC-53, TC-54", Note = "Gộp chọn câu hỏi và sắp xếp vào TC-48" });
            list.Add(new TraceItem { Code = "T-23", Name = "Kiểm thử liên hoàn Sprint 2 WBS", AcceptanceCriteria = "Chu trình xuất bản bài học và học viên vào học; Chu trình phát hành đề và tự cập nhật thống kê", TestCaseIds = "TC-55, TC-56", Note = "Kiểm tra liên hoàn Sprint 2" });
            list.Add(new TraceItem { Code = "E2E", Name = "Kiểm thử toàn trình (End-to-End)", AcceptanceCriteria = "Chu trình quản lý học liệu và thi cử từ đầu đến cuối; Quản lý xử lý vi phạm tài khoản và bảo mật", TestCaseIds = "TC-57, TC-58", Note = "Kiểm tra toàn trình quản trị" });

            return list;
        }
    }
}
