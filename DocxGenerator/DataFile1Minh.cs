using System;
using System.Collections.Generic;

namespace DocxGenerator
{
    public static class DataFile1Minh
    {
        public static List<TestCase> GetSprint1TestCases()
        {
            var list = new List<TestCase>();

            // TASK-101: Khởi tạo CSDL & Dữ liệu mẫu học liệu
            list.Add(new TestCase {
                Id = "TC-01", Sprint = "Sprint 1", Module = "Khởi tạo dữ liệu hệ thống (TASK-101)", TaskId = "TASK-101",
                Scenario = "Khởi tạo cơ sở dữ liệu cho hệ thống",
                Preconditions = "Máy chủ cơ sở dữ liệu SQL Server đang hoạt động bình thường.",
                TestData = "Tệp cài đặt cơ sở dữ liệu của dự án (EAI.sql)",
                Steps = "1. Mở phần mềm quản lý cơ sở dữ liệu.\n2. Thực hiện chạy tệp cài đặt cơ sở dữ liệu của dự án.\n3. Kiểm tra danh sách các bảng dữ liệu vừa được sinh ra.",
                Expected = "Hệ thống tạo thành công cơ sở dữ liệu TiengAnhAI. Toàn bộ các bảng dữ liệu về người dùng, bài học, câu hỏi, đề thi và kết quả làm bài được tạo đầy đủ, không có lỗi."
            });

            list.Add(new TestCase {
                Id = "TC-02", Sprint = "Sprint 1", Module = "Khởi tạo dữ liệu hệ thống (TASK-101)", TaskId = "TASK-101",
                Scenario = "Kiểm tra tính toàn vẹn dữ liệu khi cố tình nhập sai thông tin",
                Preconditions = "Cơ sở dữ liệu của hệ thống đã được cài đặt sẵn sàng.",
                TestData = "Thử nhập một tài khoản có mã vai trò không tồn tại trong danh mục",
                Steps = "1. Thử ghi dữ liệu người dùng có vai trò không hợp lệ vào hệ thống.\n2. Quan sát phản hồi ngăn chặn của hệ thống.",
                Expected = "Hệ thống từ chối lưu dữ liệu sai lệch, báo lỗi vi phạm ràng buộc dữ liệu. Không có bản ghi rác hay tài khoản lỗi nào được lưu vào hệ thống."
            });

            list.Add(new TestCase {
                Id = "TC-03", Sprint = "Sprint 1", Module = "Khởi tạo dữ liệu hệ thống (TASK-101)", TaskId = "TASK-101",
                Scenario = "Nạp dữ liệu mẫu ban đầu phục vụ khảo sát và học tập",
                Preconditions = "Cơ sở dữ liệu đã có đầy đủ cấu trúc các bảng.",
                TestData = "Tệp dữ liệu mẫu ban đầu (EAI_seed.sql)",
                Steps = "1. Chạy tệp nạp dữ liệu mẫu vào hệ thống.\n2. Kiểm tra danh mục các cấp độ tiếng Anh, bài khảo sát đầu vào và danh sách câu hỏi.",
                Expected = "Hệ thống nạp đầy đủ 5 cấp độ từ A1 đến C1, có sẵn bài kiểm tra năng lực đầu vào với 20 câu hỏi và các tài khoản người dùng mẫu sẵn sàng để thử nghiệm."
            });

            list.Add(new TestCase {
                Id = "TC-04", Sprint = "Sprint 1", Module = "Khởi tạo dữ liệu hệ thống (TASK-101)", TaskId = "TASK-101",
                Scenario = "Xử lý khi mất kết nối đến máy chủ cơ sở dữ liệu",
                Preconditions = "Máy chủ dữ liệu tạm thời bị tắt hoặc mất kết nối mạng.",
                TestData = "Cấu hình địa chỉ máy chủ không chính xác",
                Steps = "1. Khởi chạy ứng dụng EnglishAI khi chưa bật máy chủ dữ liệu.\n2. Quan sát phản ứng và thông báo trên màn hình ứng dụng.",
                Expected = "Ứng dụng không bị tự động thoát (văng ứng dụng). Hiển thị thông báo rõ ràng cho người dùng: 'Không thể kết nối đến máy chủ dữ liệu. Vui lòng kiểm tra lại kết nối mạng hoặc liên hệ quản trị viên'."
            });

            // TASK-102: Khởi động & Màn hình đầu tiên
            list.Add(new TestCase {
                Id = "TC-05", Sprint = "Sprint 1", Module = "Khởi động ứng dụng (TASK-102)", TaskId = "TASK-102",
                Scenario = "Khởi động ứng dụng và hiển thị màn hình đăng nhập ban đầu",
                Preconditions = "Máy chủ dữ liệu đang chạy bình thường.",
                TestData = "Nhấp đúp chuột vào biểu tượng phần mềm EnglishAI",
                Steps = "1. Mở ứng dụng từ màn hình máy tính.\n2. Quan sát thời gian tải ứng dụng và giao diện đầu tiên xuất hiện.",
                Expected = "Ứng dụng khởi động nhanh chóng trong vòng 3 giây. Màn hình đăng nhập hiển thị đầy đủ, rõ ràng và cân đối giữa màn hình."
            });

            // TASK-104: Đăng ký & Đăng nhập
            list.Add(new TestCase {
                Id = "TC-06", Sprint = "Sprint 1", Module = "Đăng ký & Đăng nhập tài khoản (TASK-104)", TaskId = "TASK-104",
                Scenario = "Đăng ký tài khoản học viên mới thành công với thông tin hợp lệ",
                Preconditions = "Đang ở màn hình đăng nhập, chuyển sang mục Đăng ký.",
                TestData = "Họ tên: 'Nguyễn Văn Minh', Email: 'minh_test@gmail.com', Mật khẩu: '123456', Xác nhận mật khẩu: '123456'",
                Steps = "1. Chọn mục 'Đăng ký tài khoản'.\n2. Nhập đầy đủ thông tin hợp lệ vào các ô.\n3. Nhấn nút 'Đăng ký'.",
                Expected = "Hệ thống thông báo: 'Tài khoản đã được tạo thành công!'. Tự động chuyển người dùng về mục Đăng nhập để sử dụng tài khoản mới."
            });

            list.Add(new TestCase {
                Id = "TC-07", Sprint = "Sprint 1", Module = "Đăng ký & Đăng nhập tài khoản (TASK-104)", TaskId = "TASK-104",
                Scenario = "Kiểm tra các trường hợp nhập sai khi đăng ký tài khoản",
                Preconditions = "Mở màn hình đăng ký tài khoản.",
                TestData = "Trường hợp 1: Để trống Họ tên, Email hoặc Mật khẩu;\nTrường hợp 2: Nhập email sai định dạng (ví dụ 'minh_abc', 'test@');\nTrường hợp 3: Nhập email đã được người khác sử dụng;\nTrường hợp 4: Mật khẩu xác nhận không trùng khớp với mật khẩu",
                Steps = "1. Lần lượt nhập thử các trường hợp thông tin sai sót nêu trên.\n2. Bấm nút 'Đăng ký' ở từng trường hợp.\n3. Quan sát thông báo lỗi của hệ thống.",
                Expected = "Hệ thống chặn đăng ký ở tất cả các trường hợp sai và hiển thị thông báo lỗi rõ ràng bên dưới ô nhập liệu giúp người dùng dễ dàng chỉnh sửa lại."
            });

            list.Add(new TestCase {
                Id = "TC-08", Sprint = "Sprint 1", Module = "Đăng ký & Đăng nhập tài khoản (TASK-104)", TaskId = "TASK-104",
                Scenario = "Đăng nhập thành công với tài khoản học viên hợp lệ",
                Preconditions = "Tài khoản học viên đang hoạt động bình thường.",
                TestData = "Email: 'sinhvien@englisheai.edu.vn', Mật khẩu: '123456'",
                Steps = "1. Nhập đúng email và mật khẩu tại màn hình đăng nhập.\n2. Nhấn nút 'Đăng nhập'.",
                Expected = "Đăng nhập thành công. Màn hình đăng nhập đóng lại và chuyển tiếp vào màn hình chính của học viên với đầy đủ thanh menu học tập."
            });

            list.Add(new TestCase {
                Id = "TC-09", Sprint = "Sprint 1", Module = "Đăng ký & Đăng nhập tài khoản (TASK-104)", TaskId = "TASK-104",
                Scenario = "Đăng nhập thất bại khi nhập sai mật khẩu hoặc tài khoản chưa đăng ký",
                Preconditions = "Đang ở màn hình đăng nhập.",
                TestData = "Trường hợp 1: Nhập mật khẩu sai ('111222');\nTrường hợp 2: Nhập email chưa từng đăng ký ('nguoidung_moi@gmail.com')",
                Steps = "1. Nhập thông tin thử nghiệm của từng trường hợp.\n2. Nhấn nút 'Đăng nhập'.",
                Expected = "Hệ thống từ chối đăng nhập và hiển thị thông báo: 'Email hoặc mật khẩu không chính xác'. Không cho phép vào bên trong ứng dụng."
            });

            list.Add(new TestCase {
                Id = "TC-10", Sprint = "Sprint 1", Module = "Đăng ký & Đăng nhập tài khoản (TASK-104)", TaskId = "TASK-104",
                Scenario = "Chuyển đổi qua lại giữa mục Đăng nhập và Đăng ký",
                Preconditions = "Mở màn hình đăng nhập.",
                TestData = "Nhấp chuột chuyển tab giao diện",
                Steps = "1. Đang ở mục Đăng nhập, bấm nút 'Đăng ký ngay'.\n2. Nhập một vài chữ rồi bấm nút 'Đăng nhập ngay' để quay lại.",
                Expected = "Giao diện chuyển đổi tức thì, các ô nhập liệu được làm mới sạch sẽ, nút bấm hiển thị đúng theo từng chức năng."
            });

            // TASK-105: Bảo mật & Phân quyền
            list.Add(new TestCase {
                Id = "TC-11", Sprint = "Sprint 1", Module = "Bảo mật & Phân quyền (TASK-105)", TaskId = "TASK-105",
                Scenario = "Kiểm tra tính an toàn của mật khẩu tài khoản người dùng",
                Preconditions = "Đăng ký một tài khoản mới với mật khẩu '123456'.",
                TestData = "Mật khẩu lúc tạo: '123456'",
                Steps = "1. Đăng ký tài khoản thành công.\n2. Kiểm tra chuỗi mật khẩu được lưu trữ bên trong cơ sở dữ liệu.",
                Expected = "Mật khẩu được mã hóa an toàn thành chuỗi ký tự bảo mật, tuyệt đối không lưu chữ thô '123456' để bảo vệ quyền riêng tư của người dùng."
            });

            list.Add(new TestCase {
                Id = "TC-12", Sprint = "Sprint 1", Module = "Bảo mật & Phân quyền (TASK-105)", TaskId = "TASK-105",
                Scenario = "Chặn đăng nhập đối với tài khoản học viên đã bị khóa",
                Preconditions = "Tài khoản học viên đã bị giảng viên hoặc quản trị viên tạm khóa.",
                TestData = "Email: 'locked_user@englisheai.edu.vn', Mật khẩu: '123456'",
                Steps = "1. Nhập thông tin tài khoản bị khóa vào màn hình đăng nhập.\n2. Nhấn nút 'Đăng nhập'.",
                Expected = "Hệ thống chặn đăng nhập và thông báo rõ ràng: 'Tài khoản của bạn đã bị khóa hoặc tạm dừng hoạt động. Vui lòng liên hệ quản trị viên'."
            });

            list.Add(new TestCase {
                Id = "TC-13", Sprint = "Sprint 1", Module = "Bảo mật & Phân quyền (TASK-105)", TaskId = "TASK-105",
                Scenario = "Đăng xuất tài khoản an toàn khỏi hệ thống",
                Preconditions = "Học viên đang đăng nhập và sử dụng ứng dụng.",
                TestData = "Bấm nút 'Đăng xuất' ở góc dưới menu bên trái",
                Steps = "1. Nhấn nút 'Đăng xuất'.\n2. Bấm 'Đồng ý' tại hộp thoại hỏi xác nhận.",
                Expected = "Màn hình chính đóng lại, hệ thống xóa phiên làm việc hiện tại và quay về màn hình đăng nhập. Người dùng không thể quay lại trang cũ khi chưa đăng nhập lại."
            });

            list.Add(new TestCase {
                Id = "TC-14", Sprint = "Sprint 1", Module = "Bảo mật & Phân quyền (TASK-105)", TaskId = "TASK-105",
                Scenario = "Kiểm tra phân quyền: Không cho học viên vào màn hình của Giảng viên",
                Preconditions = "Đang đăng nhập bằng tài khoản học viên bình thường.",
                TestData = "Thử truy cập vào các chức năng quản trị bài học của giảng viên",
                Steps = "1. Dùng tài khoản học viên thử mở các chức năng của giảng viên.\n2. Quan sát phản hồi của hệ thống.",
                Expected = "Hệ thống tự động ngăn chặn và cảnh báo: 'Bạn không có quyền truy cập vào chức năng dành cho giảng viên/quản trị viên'."
            });

            // TASK-106: Khung màn hình chính học viên
            list.Add(new TestCase {
                Id = "TC-15", Sprint = "Sprint 1", Module = "Giao diện chính học viên (TASK-106)", TaskId = "TASK-106",
                Scenario = "Hiển thị màn hình chính, thanh menu và thông tin người dùng",
                Preconditions = "Đăng nhập bằng tài khoản học viên 'Nguyễn Văn Minh'.",
                TestData = "Tài khoản: 'Nguyễn Văn Minh'",
                Steps = "1. Đăng nhập vào màn hình chính.\n2. Quan sát thanh menu bên trái và góc thông tin cá nhân bên trên.",
                Expected = "Menu hiển thị đầy đủ các mục: Trang chủ, Lộ trình học, Luyện tập, Thi thử, Khảo sát, Đăng xuất. Ảnh đại diện hiển thị chữ cái viết tắt họ tên 'NM' rõ ràng, đẹp mắt."
            });

            list.Add(new TestCase {
                Id = "TC-16", Sprint = "Sprint 1", Module = "Giao diện chính học viên (TASK-106)", TaskId = "TASK-106",
                Scenario = "Chuyển đổi giữa các mục menu và làm nổi bật mục đang chọn",
                Preconditions = "Màn hình chính đang hiển thị.",
                TestData = "Nhấp chuột lần lượt: Trang chủ -> Lộ trình học -> Luyện tập -> Thi thử",
                Steps = "1. Bấm chuột vào từng mục trên thanh menu bên trái.\n2. Quan sát nội dung trang hiển thị tương ứng ở giữa màn hình.",
                Expected = "Màn hình chuyển đổi đúng nội dung của từng mục, nút menu đang chọn được làm nổi bật để người dùng dễ nhận biết vị trí đang xem."
            });

            list.Add(new TestCase {
                Id = "TC-17", Sprint = "Sprint 1", Module = "Giao diện chính học viên (TASK-106)", TaskId = "TASK-106",
                Scenario = "Khả năng co giãn giao diện khi thay đổi kích thước cửa sổ",
                Preconditions = "Ứng dụng đang mở ở kích thước mặc định.",
                TestData = "Thao tác phóng to toàn màn hình và thu nhỏ cửa sổ",
                Steps = "1. Nhấn nút phóng to toàn màn hình (Maximize).\n2. Thu nhỏ cửa sổ ứng dụng về kích thước nhỏ hơn.",
                Expected = "Bố cục các ô thống kê và danh sách bài học tự động căn chỉnh ngay ngắn, không bị tràn chữ hay che khuất các nút bấm."
            });

            // TASK-107: Quy trình cho người dùng mới (Onboarding)
            list.Add(new TestCase {
                Id = "TC-18", Sprint = "Sprint 1", Module = "Quy trình khảo sát đầu vào (TASK-107)", TaskId = "TASK-107",
                Scenario = "Người dùng đăng nhập lần đầu bắt buộc phải làm bài khảo sát năng lực",
                Preconditions = "Tài khoản học viên mới đăng ký lần đầu tiên.",
                TestData = "Tài khoản mới tạo: minh_test@gmail.com",
                Steps = "1. Đăng nhập bằng tài khoản mới vừa tạo.\n2. Quan sát màn hình xuất hiện ngay sau khi đăng nhập.",
                Expected = "Thanh menu bên trái được tạm ẩn để người học tập trung. Màn hình đưa thẳng vào bài khảo sát năng lực đầu vào kèm lời chào và hướng dẫn làm bài."
            });

            list.Add(new TestCase {
                Id = "TC-19", Sprint = "Sprint 1", Module = "Quy trình khảo sát đầu vào (TASK-107)", TaskId = "TASK-107",
                Scenario = "Mở khóa toàn bộ chức năng sau khi hoàn thành bài khảo sát đầu vào",
                Preconditions = "Học viên mới đang làm bài khảo sát đầu vào.",
                TestData = "Hoàn thành và nộp bài khảo sát",
                Steps = "1. Trả lời các câu hỏi và nhấn nút 'Nộp bài'.\n2. Xem kết quả xếp lớp và bấm nút 'Bắt đầu học ngay'.",
                Expected = "Hệ thống ghi nhận kết quả xếp lớp, thanh menu bên trái tự động xuất hiện trở lại và mở khóa đầy đủ tất cả các chức năng học tập."
            });

            list.Add(new TestCase {
                Id = "TC-20", Sprint = "Sprint 1", Module = "Quy trình khảo sát đầu vào (TASK-107)", TaskId = "TASK-107",
                Scenario = "Tài khoản đã hoàn thành kiểm tra đầu vào vào thẳng Trang chủ",
                Preconditions = "Tài khoản học viên cũ đã hoàn thành bài khảo sát từ trước.",
                TestData = "Tài khoản: sinhvien@englisheai.edu.vn",
                Steps = "1. Đăng nhập vào hệ thống.\n2. Quan sát màn hình hiển thị.",
                Expected = "Hệ thống đưa thẳng vào Trang chủ học tập với đầy đủ thanh menu, không bắt buộc người dùng phải làm lại bài khảo sát đầu vào."
            });

            list.Add(new TestCase {
                Id = "TC-21", Sprint = "Sprint 1", Module = "Quy trình khảo sát đầu vào (TASK-107)", TaskId = "TASK-107",
                Scenario = "Xử lý khi học viên tắt ứng dụng giữa chừng lúc đang làm bài khảo sát",
                Preconditions = "Học viên mới đang làm dở bài khảo sát đầu vào.",
                TestData = "Thao tác: Bấm nút đóng cửa sổ [X] khi đang làm bài",
                Steps = "1. Đang làm bài khảo sát ở câu số 5, bấm tắt ứng dụng.\n2. Mở lại ứng dụng và đăng nhập lại bằng chính tài khoản đó.",
                Expected = "Do chưa nộp bài hoàn tất, hệ thống tiếp tục đưa người học vào màn hình khảo sát đầu vào để hoàn thành việc xếp lớp trước khi sử dụng các chức năng khác."
            });

            // TASK-108: Giao diện Khảo sát năng lực
            list.Add(new TestCase {
                Id = "TC-22", Sprint = "Sprint 1", Module = "Khảo sát năng lực đầu vào (TASK-108)", TaskId = "TASK-108",
                Scenario = "Tải đầy đủ danh sách câu hỏi kiểm tra năng lực đầu vào",
                Preconditions = "Mở trang khảo sát năng lực.",
                TestData = "Bài kiểm tra đầu vào của hệ thống",
                Steps = "1. Vào màn hình khảo sát năng lực.\n2. Kiểm tra danh sách câu hỏi và bảng danh sách số câu.",
                Expected = "Hệ thống tải đầy đủ 15 đến 20 câu hỏi bao quát ngữ pháp, từ vựng và đọc hiểu. Bảng số thứ tự câu hỏi (từ 1 đến 20) hiển thị rõ ràng bên cạnh."
            });

            list.Add(new TestCase {
                Id = "TC-23", Sprint = "Sprint 1", Module = "Khảo sát năng lực đầu vào (TASK-108)", TaskId = "TASK-108",
                Scenario = "Chọn đáp án, đổi đáp án và chuyển câu hỏi khi làm bài khảo sát",
                Preconditions = "Đang ở màn hình làm bài khảo sát.",
                TestData = "Chọn đáp án A, đổi sang B; bấm nút 'Câu tiếp' và bấm chọn câu trên bảng số",
                Steps = "1. Chọn phương án A cho câu 1, sau đó đổi sang phương án B.\n2. Bấm nút 'Câu tiếp' để sang câu 2.\n3. Nhấp trực tiếp vào ô số 10 trên bảng số câu.\n4. Bấm quay lại câu 1 kiểm tra.",
                Expected = "Đáp án đã chọn hiển thị rõ ràng, ô số tương ứng trên bảng đổi màu báo hiệu câu đã làm xong. Khi quay lại câu 1, đáp án B vẫn được lưu giữ nguyên vẹn."
            });

            list.Add(new TestCase {
                Id = "TC-24", Sprint = "Sprint 1", Module = "Khảo sát năng lực đầu vào (TASK-108)", TaskId = "TASK-108",
                Scenario = "Cảnh báo khi người học nhấn Nộp bài nhưng chưa làm hết câu hỏi",
                Preconditions = "Đang làm bài khảo sát 20 câu, mới trả lời 12 câu (còn 8 câu chưa làm).",
                TestData = "Bấm nút 'Nộp bài'",
                Steps = "1. Mới làm 12 câu, bấm nút 'Nộp bài'.\n2. Đọc nội dung thông báo trên hộp thoại xác nhận.",
                Expected = "Xuất hiện hộp thoại nhắc nhở: 'Bạn còn 8 câu chưa trả lời. Bạn có chắc chắn muốn nộp bài đánh giá năng lực ngay bây giờ không?'. Cho phép người dùng chọn làm tiếp hoặc đồng ý nộp bài."
            });

            list.Add(new TestCase {
                Id = "TC-25", Sprint = "Sprint 1", Module = "Khảo sát năng lực đầu vào (TASK-108)", TaskId = "TASK-108",
                Scenario = "Bảo mật: Không để lộ đáp án đúng trong quá trình đang làm bài",
                Preconditions = "Học viên đang làm bài khảo sát đầu vào.",
                TestData = "Kiểm tra thông tin hiển thị trên màn hình",
                Steps = "1. Quan sát kỹ màn hình câu hỏi và các phương án trong lúc làm bài.\n2. Xác nhận không có dấu hiệu nhận biết đáp án đúng.",
                Expected = "Toàn bộ thông tin đáp án đúng được bảo mật tuyệt đối, chỉ hiển thị kết quả và giải thích sau khi người học đã bấm nộp bài hoàn tất."
            });

            // TASK-109: Đánh giá & Xếp loại CEFR
            list.Add(new TestCase {
                Id = "TC-26", Sprint = "Sprint 1", Module = "Đánh giá trình độ & Hồ sơ (TASK-109)", TaskId = "TASK-109",
                Scenario = "Tính tổng điểm và số câu đúng bài khảo sát chính xác",
                Preconditions = "Thí sinh làm bài khảo sát 20 câu, trả lời đúng chính xác 14 câu.",
                TestData = "14 câu đúng trên tổng số 20 câu",
                Steps = "1. Nộp bài khảo sát với 14 câu trả lời đúng.\n2. Quan sát kết quả hiển thị trên màn hình.",
                Expected = "Hệ thống ghi nhận chính xác: Số câu đúng là 14/20 câu, điểm số tương ứng đạt 7.0 trên thang điểm 10.0."
            });

            list.Add(new TestCase {
                Id = "TC-27", Sprint = "Sprint 1", Module = "Đánh giá trình độ & Hồ sơ (TASK-109)", TaskId = "TASK-109",
                Scenario = "Xếp loại trình độ học viên (A1, A2, B1, B2, C1) theo thang điểm chuẩn",
                Preconditions = "Thang điểm chuẩn: 0-4.0đ (A1), 4.5-6.0đ (A2), 6.5-7.5đ (B1), 8.0-9.0đ (B2), 9.5-10.0đ (C1).",
                TestData = "Kiểm tra các mức điểm: 4.0đ, 4.5đ, 6.5đ, 8.0đ, 9.5đ",
                Steps = "1. Kiểm tra kết quả xếp loại tự động với từng mức điểm thử nghiệm.\n2. Đối chiếu cấp độ nhận được.",
                Expected = "Hệ thống xếp đúng cấp độ: 4.0đ là A1; 4.5đ là A2; 6.5đ là B1; 8.0đ là B2; 9.5đ là C1. Không xảy ra lỗi sai lệch cấp độ ở các mốc ranh giới."
            });

            list.Add(new TestCase {
                Id = "TC-28", Sprint = "Sprint 1", Module = "Đánh giá trình độ & Hồ sơ (TASK-109)", TaskId = "TASK-109",
                Scenario = "Lưu trình độ và điểm số đầu vào vào hồ sơ của học viên",
                Preconditions = "Học viên hoàn thành bài khảo sát đạt trình độ B1 (7.0 điểm).",
                TestData = "Điểm: 7.0, Trình độ: B1",
                Steps = "1. Nộp bài khảo sát và nhận kết quả.\n2. Mở xem thông tin hồ sơ học tập của học viên.",
                Expected = "Hồ sơ của học viên được cập nhật chính xác: Trình độ đầu vào là B1 và điểm đánh giá đầu vào là 7.0 điểm."
            });

            list.Add(new TestCase {
                Id = "TC-29", Sprint = "Sprint 1", Module = "Đánh giá trình độ & Hồ sơ (TASK-109)", TaskId = "TASK-109",
                Scenario = "Xử lý khi nộp bài khảo sát mà không chọn câu trả lời nào (0 điểm)",
                Preconditions = "Học viên mở bài khảo sát và bấm nộp bài ngay mà không chọn đáp án nào.",
                TestData = "Số câu trả lời = 0",
                Steps = "1. Bấm nút 'Nộp bài' và xác nhận đồng ý nộp bài trống.\n2. Quan sát kết quả xếp loại.",
                Expected = "Hệ thống ghi nhận 0.0 điểm, tự động xếp mức độ cơ bản ban đầu là A1 (Mất gốc) để học lại từ đầu, ứng dụng hoạt động bình thường không bị lỗi."
            });

            // TASK-110: Tích hợp Sprint 1
            list.Add(new TestCase {
                Id = "TC-30", Sprint = "Sprint 1", Module = "Kiểm thử liên hoàn Sprint 1 (TASK-110)", TaskId = "TASK-110",
                Scenario = "Kiểm tra liên hoàn từ Đăng ký, Đăng nhập, Làm khảo sát đến vào Trang chủ",
                Preconditions = "Ứng dụng đang ở màn hình đăng nhập.",
                TestData = "Chuỗi thao tác liên tục của người dùng mới",
                Steps = "1. Đăng ký tài khoản học viên mới.\n2. Đăng nhập vào hệ thống.\n3. Làm bài kiểm tra năng lực đầu vào 20 câu.\n4. Nộp bài, xem kết quả xếp lớp B1.\n5. Bấm vào Trang chủ bắt đầu học.",
                Expected = "Toàn bộ chu trình diễn ra liền mạch, dữ liệu được lưu trữ chính xác, tài khoản được kích hoạt đầy đủ chức năng sau khi xong bài kiểm tra."
            });

            list.Add(new TestCase {
                Id = "TC-31", Sprint = "Sprint 1", Module = "Kiểm thử liên hoàn Sprint 1 (TASK-110)", TaskId = "TASK-110",
                Scenario = "Đăng xuất và đăng nhập lại sau khi đã hoàn thành khảo sát đầu vào",
                Preconditions = "Học viên vừa hoàn thành bài khảo sát đầu vào.",
                TestData = "Đăng xuất và đăng nhập lại",
                Steps = "1. Bấm 'Đăng xuất' khỏi hệ thống.\n2. Nhập lại email và mật khẩu của tài khoản vừa xong khảo sát để đăng nhập lại.",
                Expected = "Hệ thống đưa thẳng vào Trang chủ với đầy đủ thanh menu, huy hiệu trình độ hiển thị đúng là B1, không xuất hiện lại màn hình khảo sát bắt buộc."
            });

            return list;
        }

        public static List<TestCase> GetSprint2TestCases()
        {
            var list = new List<TestCase>();

            // TASK-201: Trang chủ Sinh viên
            list.Add(new TestCase {
                Id = "TC-32", Sprint = "Sprint 2", Module = "Trang chủ học viên (TASK-201)", TaskId = "TASK-201",
                Scenario = "Hiển thị lời chào cá nhân và huy hiệu trình độ của học viên",
                Preconditions = "Đăng nhập tài khoản học viên 'Nguyễn Văn Minh' có trình độ B1.",
                TestData = "Họ tên: 'Nguyễn Văn Minh', Trình độ: 'B1'",
                Steps = "1. Đăng nhập vào ứng dụng và mở Trang chủ.\n2. Quan sát phần banner trên cùng của màn hình.",
                Expected = "Hiển thị đúng lời chào cá nhân: 'Xin chào, Nguyễn Văn Minh! Chúc bạn một ngày học tập hiệu quả' kèm huy hiệu trình độ hiện tại 'B1 - Intermediate'."
            });

            list.Add(new TestCase {
                Id = "TC-33", Sprint = "Sprint 2", Module = "Trang chủ học viên (TASK-201)", TaskId = "TASK-201",
                Scenario = "Hiển thị 3 thẻ thống kê học tập: Bài đã học, Thời gian học và Điểm thi",
                Preconditions = "Học viên đã học 12 bài, tích lũy 180 phút học và điểm thi trung bình là 7.8.",
                TestData = "Dữ liệu học tập thực tế của học viên",
                Steps = "1. Mở xem Trang chủ.\n2. Kiểm tra số liệu tại 3 thẻ thống kê trên màn hình.",
                Expected = "Cả 3 thẻ hiển thị chính xác số liệu: Số bài học hoàn thành là 12 bài; Tổng thời gian học là 180 phút (3.0 giờ); Điểm thi trung bình là 7.8 điểm."
            });

            list.Add(new TestCase {
                Id = "TC-34", Sprint = "Sprint 2", Module = "Trang chủ học viên (TASK-201)", TaskId = "TASK-201",
                Scenario = "Hiển thị danh sách bài học gần đây và thể hiện rõ ràng tiêu đề bài học",
                Preconditions = "Học viên đã có học các bài học trong thời gian gần nhất.",
                TestData = "Danh sách bài học gần đây, có bài có tiêu đề dài trên 40 ký tự",
                Steps = "1. Quan sát khu vực 'Bài học gần đây' trên Trang chủ.\n2. Xem các thẻ bài học có tiêu đề dài.",
                Expected = "Các bài học hiển thị theo thứ tự bài mới học lên đầu. Tiêu đề hiển thị trọn vẹn, rõ ràng, không bị đè chữ lên nhãn kỹ năng hay nút bấm."
            });

            list.Add(new TestCase {
                Id = "TC-35", Sprint = "Sprint 2", Module = "Trang chủ học viên (TASK-201)", TaskId = "TASK-201",
                Scenario = "Nút bấm nhanh 'Học ngay' và 'Thi thử ngay' trên Trang chủ",
                Preconditions = "Đang ở Trang chủ của ứng dụng.",
                TestData = "Bấm nút 'Học ngay' trên banner và nút 'Thi thử ngay' trên thẻ lối tắt",
                Steps = "1. Nhấn nút 'Học ngay' trên màn hình. Quan sát màn hình mở ra.\n2. Quay lại Trang chủ, nhấn nút 'Thi thử ngay'.",
                Expected = "Nút 'Học ngay' mở nhanh đến bài học kế tiếp trong lộ trình. Nút 'Thi thử ngay' mở nhanh đến danh sách đề thi thử."
            });

            // TASK-202: Học tập theo Lộ trình
            list.Add(new TestCase {
                Id = "TC-36", Sprint = "Sprint 2", Module = "Học tập theo Lộ trình (TASK-202)", TaskId = "TASK-202",
                Scenario = "Tải toàn bộ danh sách bài học theo chủ đề và trình độ",
                Preconditions = "Hệ thống có sẵn các bài học đã xuất bản.",
                TestData = "Mở mục 'Lộ trình học' trên menu bên trái",
                Steps = "1. Bấm chuột vào mục 'Lộ trình học'.\n2. Quan sát danh mục các bài học hiển thị trên màn hình.",
                Expected = "Toàn bộ bài học phù hợp với trình độ của học viên được hiển thị ngăn nắp theo từng chủ đề kèm tên bài, kỹ năng, thời lượng dự kiến và trạng thái hoàn thành."
            });

            list.Add(new TestCase {
                Id = "TC-37", Sprint = "Sprint 2", Module = "Học tập theo Lộ trình (TASK-202)", TaskId = "TASK-202",
                Scenario = "Lọc bài học theo các kỹ năng cốt lõi (Nghe, Nói, Đọc, Viết, Ngữ pháp)",
                Preconditions = "Đang ở trang Lộ trình học.",
                TestData = "Bấm các nút lọc kỹ năng: 'Nghe', 'Ngữ pháp', 'Từ vựng', 'Tất cả'",
                Steps = "1. Bấm nút lọc 'Nghe'. Kiểm tra danh sách bài học.\n2. Bấm nút lọc 'Ngữ pháp'.\n3. Bấm lại nút 'Tất cả'.",
                Expected = "Danh sách bài học lọc nhanh chóng chỉ hiển thị đúng kỹ năng đã chọn. Khi chọn 'Tất cả', toàn bộ bài học hiển thị lại đầy đủ."
            });

            list.Add(new TestCase {
                Id = "TC-38", Sprint = "Sprint 2", Module = "Học tập theo Lộ trình (TASK-202)", TaskId = "TASK-202",
                Scenario = "Mở xem nội dung chi tiết bài học ngữ pháp hoặc từ vựng",
                Preconditions = "Chọn một bài học ngữ pháp (ví dụ: 'Thì hiện tại hoàn thành').",
                TestData = "Bấm nút 'Bắt đầu học' tại thẻ bài học",
                Steps = "1. Nhấn nút 'Bắt đầu học'.\n2. Đọc nội dung bài giảng hiển thị trên màn hình.",
                Expected = "Trình đọc bài học hiển thị rõ ràng: định nghĩa, công thức cấu trúc câu, bảng ví dụ minh họa và các lưu ý ngữ pháp quan trọng. Cuộn trang mượt mà."
            });

            list.Add(new TestCase {
                Id = "TC-39", Sprint = "Sprint 2", Module = "Học tập theo Lộ trình (TASK-202)", TaskId = "TASK-202",
                Scenario = "Giữ nguyên bộ lọc đang chọn khi quay lại danh sách bài học",
                Preconditions = "Đang lọc danh sách bài học theo kỹ năng 'Nghe'.",
                TestData = "Mở 1 bài nghe rồi bấm quay lại",
                Steps = "1. Đang lọc bài 'Nghe', bấm mở 1 bài nghe để học.\n2. Nhấn nút 'Quay lại danh sách'.\n3. Kiểm tra bộ lọc trên màn hình.",
                Expected = "Màn hình quay về danh sách bài học và vẫn giữ nguyên bộ lọc 'Nghe' đang chọn, không bị đặt lại về trạng thái ban đầu."
            });

            // TASK-203: Trình phát Audio bài nghe
            list.Add(new TestCase {
                Id = "TC-40", Sprint = "Sprint 2", Module = "Trình phát âm thanh bài nghe (TASK-203)", TaskId = "TASK-203",
                Scenario = "Phát, tạm dừng, tua và đổi tốc độ phát âm thanh bài nghe",
                Preconditions = "Mở bài học kỹ năng Nghe có file âm thanh.",
                TestData = "Thử các thao tác: Phát (Play), Tạm dừng (Pause), Tua thời gian và chọn tốc độ 0.75x, 1.25x",
                Steps = "1. Nhấn nút 'Phát' để nghe âm thanh bài giảng.\n2. Bấm 'Tạm dừng' rồi bấm phát tiếp.\n3. Kéo thanh thời gian để tua đến giây thứ 30.\n4. Chọn các nấc tốc độ 0.75x và 1.25x.",
                Expected = "Âm thanh phát rõ ràng, khớp với thời gian hiển thị. Tạm dừng và phát tiếp mượt mà. Tua âm thanh chuẩn xác. Đổi tốc độ đọc tự nhiên, không bị méo tiếng."
            });

            list.Add(new TestCase {
                Id = "TC-41", Sprint = "Sprint 2", Module = "Trình phát âm thanh bài nghe (TASK-203)", TaskId = "TASK-203",
                Scenario = "Tính năng tự động đọc văn bản bài nghe khi chưa có sẵn file ghi âm",
                Preconditions = "Bài học nghe chưa có file ghi âm đính kèm sẵn.",
                TestData = "Đoạn văn bản bài nghe",
                Steps = "1. Mở bài nghe chưa có file ghi âm.\n2. Bấm nút 'Phát' âm thanh.\n3. Lắng nghe giọng đọc phát ra.",
                Expected = "Hệ thống tự động kích hoạt tính năng đọc văn bản thành giọng nói tiếng Anh chuẩn, giúp học viên vẫn nghe và luyện tập được bình thường."
            });

            list.Add(new TestCase {
                Id = "TC-42", Sprint = "Sprint 2", Module = "Trình phát âm thanh bài nghe (TASK-203)", TaskId = "TASK-203",
                Scenario = "Dừng ngay âm thanh bài cũ khi chuyển sang bài học khác",
                Preconditions = "Âm thanh bài nghe đang phát dở.",
                TestData = "Thao tác chuyển bài học hoặc chuyển mục menu",
                Steps = "1. Bật âm thanh bài 1 đang phát.\n2. Nhấn nút chuyển sang bài học khác hoặc bấm sang mục 'Luyện tập'.\n3. Lắng nghe âm thanh.",
                Expected = "Âm thanh của bài cũ dừng phát ngay lập tức, hoàn toàn không có tình trạng âm thanh chạy ngầm hay phát đè lên bài học mới."
            });

            // TASK-204: Ghi nhận Thời gian & Tiến độ
            list.Add(new TestCase {
                Id = "TC-43", Sprint = "Sprint 2", Module = "Ghi nhận tiến độ học tập (TASK-204)", TaskId = "TASK-204",
                Scenario = "Tự động cộng dồn thời gian học thực tế vào hồ sơ học viên",
                Preconditions = "Học viên mở bài học và học tập liên tục trong 5 phút.",
                TestData = "Thời gian học tập: 5 phút",
                Steps = "1. Mở bài học đọc lý thuyết và nghe bài trong 5 phút.\n2. Bấm nút 'Hoàn thành' hoặc chuyển bài.\n3. Kiểm tra thời lượng học tích lũy trên Trang chủ.",
                Expected = "Tổng thời gian học của học viên được cộng dồn thêm đúng 5 phút học thực tế, chỉ số thời gian trên Trang chủ cập nhật số mới."
            });

            list.Add(new TestCase {
                Id = "TC-44", Sprint = "Sprint 2", Module = "Ghi nhận tiến độ học tập (TASK-204)", TaskId = "TASK-204",
                Scenario = "Ghi nhận tiến độ 100% sau khi hoàn thành lý thuyết và bài tập củng cố",
                Preconditions = "Học viên đọc xong lý thuyết và làm bài tập củng cố ở cuối bài.",
                TestData = "Làm xong bài tập củng cố đạt yêu cầu",
                Steps = "1. Học xong lý thuyết và làm bài tập củng cố đạt yêu cầu.\n2. Nhấn nút 'Xác nhận hoàn thành bài học'.\n3. Quan sát trạng thái bài học.",
                Expected = "Bài học được đánh dấu hoàn thành 100% kèm biểu tượng tích xanh. Chỉ số 'Bài học hoàn thành' trên Trang chủ tăng thêm 1 bài."
            });

            list.Add(new TestCase {
                Id = "TC-45", Sprint = "Sprint 2", Module = "Ghi nhận tiến độ học tập (TASK-204)", TaskId = "TASK-204",
                Scenario = "Chỉ ghi nhận tiến độ một phần khi chưa hoàn thành bài tập củng cố",
                Preconditions = "Học viên chỉ đọc lý thuyết trong 3 phút, chưa làm bài tập củng cố.",
                TestData = "Thoát ra ngoài khi chưa làm bài tập cuối bài",
                Steps = "1. Xem lý thuyết 3 phút rồi bấm thoát ra ngoài danh sách bài học.\n2. Kiểm tra tiến độ của bài học đó.",
                Expected = "Bài học chỉ ghi nhận trạng thái 'Đang học' (50%), hệ thống không cho phép tính là đã hoàn thành 100% khi chưa làm bài tập củng cố."
            });

            list.Add(new TestCase {
                Id = "TC-46", Sprint = "Sprint 2", Module = "Ghi nhận tiến độ học tập (TASK-204)", TaskId = "TASK-204",
                Scenario = "Không cộng thời gian khi người học mở bài rồi thoát ra ngay dưới 10 giây",
                Preconditions = "Học viên nhấp mở bài học rồi bấm thoát ra ngay trong vòng 5 giây.",
                TestData = "Thời gian mở bài: 5 giây",
                Steps = "1. Nhấp mở bài học, sau 5 giây bấm thoát ra ngay.\n2. Kiểm tra lại thời gian tích lũy trên Trang chủ.",
                Expected = "Hệ thống tự động lọc bỏ các lượt mở quá nhanh dưới 10 giây, không tính thời gian học ảo nhằm đảm bảo tính trung thực trong học tập."
            });

            // TASK-205: Phân hệ Luyện tập theo Kỹ năng
            list.Add(new TestCase {
                Id = "TC-47", Sprint = "Sprint 2", Module = "Luyện tập theo kỹ năng (TASK-205)", TaskId = "TASK-205",
                Scenario = "Tải danh sách câu hỏi luyện tập theo từng kỹ năng và trình độ",
                Preconditions = "Mở mục 'Luyện tập' trên menu.",
                TestData = "Chọn kỹ năng 'Từ vựng' và trình độ 'B1'",
                Steps = "1. Chọn kỹ năng 'Từ vựng' và trình độ 'B1'.\n2. Nhấn nút 'Bắt đầu luyện tập'.",
                Expected = "Hệ thống tải đúng danh sách câu hỏi trắc nghiệm từ vựng phù hợp với trình độ B1. Màn hình hiển thị câu hỏi, các phương án và số câu hiện tại."
            });

            list.Add(new TestCase {
                Id = "TC-48", Sprint = "Sprint 2", Module = "Luyện tập theo kỹ năng (TASK-205)", TaskId = "TASK-205",
                Scenario = "Luyện tập trắc nghiệm: Báo đúng/sai tức thì và hiển thị giải thích chi tiết",
                Preconditions = "Đang ở màn hình luyện tập câu hỏi trắc nghiệm.",
                TestData = "Chọn đáp án đúng (A) hoặc chọn đáp án sai (B)",
                Steps = "1. Nhấp chọn đáp án A (đáp án đúng). Quan sát màu sắc.\n2. Sang câu tiếp theo, nhấp chọn đáp án B (đáp án sai). Quan sát phản hồi.",
                Expected = "Chọn đúng hiển thị viền xanh kèm dấu tích; chọn sai hiển thị viền đỏ và làm nổi bật đáp án đúng màu xanh. Phía dưới có khung giải thích ngữ pháp chi tiết."
            });

            list.Add(new TestCase {
                Id = "TC-49", Sprint = "Sprint 2", Module = "Luyện tập theo kỹ năng (TASK-205)", TaskId = "TASK-205",
                Scenario = "Học từ vựng qua thẻ lật thông minh kèm phiên âm và phát âm",
                Preconditions = "Chọn chế độ học thẻ từ vựng Flashcard.",
                TestData = "Bộ từ vựng chủ đề công việc",
                Steps = "1. Nhấp chuột vào thẻ để lật qua lại giữa mặt trước và mặt sau.\n2. Nhấn nút phát âm thanh từ vựng.",
                Expected = "Mặt trước hiển thị từ tiếng Anh, từ loại và phiên âm chuẩn. Mặt sau hiển thị nghĩa tiếng Việt và câu ví dụ. Nút phát âm đọc chuẩn xác từ vựng."
            });

            list.Add(new TestCase {
                Id = "TC-50", Sprint = "Sprint 2", Module = "Luyện tập theo kỹ năng (TASK-205)", TaskId = "TASK-205",
                Scenario = "Luyện nghe chép chính tả và tính điểm phần trăm độ chính xác",
                Preconditions = "Chọn bài luyện nghe chép câu tiếng Anh.",
                TestData = "Nghe câu mẫu và gõ lại, cố ý gõ thiếu 1 từ",
                Steps = "1. Nghe đoạn âm thanh đọc câu mẫu.\n2. Gõ lại câu nghe được vào ô nhập liệu.\n3. Nhấn nút 'Kiểm tra'.",
                Expected = "Hệ thống tính chính xác tỷ lệ đúng theo phần trăm (ví dụ: 88%) và đánh dấu màu nổi bật từ bị gõ thiếu để người học nhận biết."
            });

            // TASK-206: Động cơ Thi thử Chuẩn hóa
            list.Add(new TestCase {
                Id = "TC-51", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Xem danh sách các đề thi thử chuẩn hóa (TOEIC, IELTS, Mini-Test)",
                Preconditions = "Hệ thống có các đề thi đã phát hành sẵn.",
                TestData = "Mở mục 'Thi thử' trên menu bên trái",
                Steps = "1. Bấm chuột vào mục 'Thi thử'.\n2. Quan sát danh mục đề thi hiển thị.",
                Expected = "Danh mục hiển thị đầy đủ: Tên đề thi, loại đề thi, số lượng câu hỏi và thời gian làm bài quy định (phút)."
            });

            list.Add(new TestCase {
                Id = "TC-52", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Làm bài thi thử: Chọn đáp án, cắm cờ câu khó và giữ nguyên câu trả lời",
                Preconditions = "Học viên nhấn 'Bắt đầu làm bài' một đề thi.",
                TestData = "Trả lời câu 1, cắm cờ câu 1, chuyển sang câu khác rồi quay lại câu 1",
                Steps = "1. Chọn đáp án cho câu 1.\n2. Bấm nút 'Cắm cờ' để đánh dấu câu cần xem lại sau.\n3. Chuyển sang câu khác rồi bấm quay lại câu 1.",
                Expected = "Đáp án đã chọn ở câu 1 vẫn được giữ nguyên vẹn. Trên bảng số câu có biểu tượng lá cờ nhỏ đánh dấu câu hỏi đó."
            });

            list.Add(new TestCase {
                Id = "TC-53", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Đồng hồ đếm ngược thời gian làm bài thi chạy chuẩn xác từng giây",
                Preconditions = "Đề thi có thời gian quy định 45 phút.",
                TestData = "Đồng hồ đếm ngược từ 45:00",
                Steps = "1. Bắt đầu làm bài thi.\n2. Theo dõi đồng hồ đếm ngược ở góc màn hình trong vòng 1 phút.",
                Expected = "Đồng hồ đếm lùi đều đặn từng giây (44:59, 44:58...). Khi thời gian còn dưới 5 phút, đồng hồ chuyển sang màu đỏ để nhắc nhở thí sinh."
            });

            list.Add(new TestCase {
                Id = "TC-54", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Nộp bài thi thủ công kèm hộp thoại xác nhận số câu chưa làm",
                Preconditions = "Đang làm bài thi 40 câu, đã làm 35 câu (còn 5 câu chưa làm).",
                TestData = "Bấm nút 'Nộp bài'",
                Steps = "1. Nhấn nút 'Nộp bài'.\n2. Đọc thông báo trên hộp thoại xác nhận.\n3. Bấm 'Đồng ý nộp'.",
                Expected = "Hộp thoại nhắc nhở rõ ràng: 'Bạn đã làm 35/40 câu (còn 5 câu chưa làm). Bạn có chắc chắn muốn nộp bài?'. Bấm đồng ý thì bài thi được thu ngay và chuyển sang chấm điểm."
            });

            list.Add(new TestCase {
                Id = "TC-55", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Tự động thu bài và nộp bài khi đồng hồ đếm ngược về 00:00",
                Preconditions = "Thí sinh đang làm bài thi và thời gian làm bài đếm ngược về 00:00.",
                TestData = "Thời gian hết giờ (00:00)",
                Steps = "1. Làm bài đến khi đồng hồ đếm ngược điểm 00:00.\n2. Quan sát phản ứng tự động của ứng dụng.",
                Expected = "Hệ thống tự động khóa bài thi, thông báo hết giờ và nộp toàn bộ các câu đã làm lên hệ thống để chấm điểm, thí sinh không thể chỉnh sửa thêm."
            });

            list.Add(new TestCase {
                Id = "TC-56", Sprint = "Sprint 2", Module = "Thi thử chuẩn hóa (TASK-206)", TaskId = "TASK-206",
                Scenario = "Chặn nộp bài trùng lặp khi bấm nhanh nhiều lần nút nộp bài",
                Preconditions = "Đang ở màn hình làm bài thi.",
                TestData = "Bấm chuột liên tiếp 3 lần thật nhanh vào nút 'Nộp bài'",
                Steps = "1. Nhấn liên tục nhiều lần vào nút 'Nộp bài'.\n2. Quan sát phản hồi của nút bấm.",
                Expected = "Nút nộp bài bị khóa ngay sau lần bấm đầu tiên kèm thông báo 'Đang chấm điểm...', hệ thống chỉ nhận đúng 1 bài nộp duy nhất, không bị nhân đôi kết quả."
            });

            // TASK-207: Chấm điểm & Lưu lịch sử thi
            list.Add(new TestCase {
                Id = "TC-57", Sprint = "Sprint 2", Module = "Chấm điểm & Lưu kết quả thi (TASK-207)", TaskId = "TASK-207",
                Scenario = "Tự động chấm điểm bài thi quy đổi theo thang điểm 10.0 chuẩn",
                Preconditions = "Đề thi gồm 40 câu hỏi trắc nghiệm.",
                TestData = "Trường hợp 1: Đúng 40/40 câu (100%);\nTrường hợp 2: Đúng 20/40 câu (50%);\nTrường hợp 3: Đúng 0/40 câu (0%)",
                Steps = "1. Kiểm tra điểm số được chấm với từng trường hợp thử nghiệm trên.\n2. Quan sát điểm số tính toán hiển thị.",
                Expected = "Điểm số tính chính xác tuyệt đối: Đúng 40/40 câu đạt 10.0 điểm; Đúng 20/40 câu đạt 5.0 điểm; Đúng 0/40 câu đạt 0.0 điểm."
            });

            list.Add(new TestCase {
                Id = "TC-58", Sprint = "Sprint 2", Module = "Chấm điểm & Lưu kết quả thi (TASK-207)", TaskId = "TASK-207",
                Scenario = "Lưu kết quả điểm số và thời gian làm bài vào lịch sử thi",
                Preconditions = "Thí sinh hoàn thành nộp bài thi.",
                TestData = "Nộp bài thi mã số 1",
                Steps = "1. Nộp bài thi.\n2. Kiểm tra thông tin kết quả vừa được lưu trong hệ thống.",
                Expected = "Lưu trữ đầy đủ thông tin: Tên đề thi, điểm số, số câu đúng, ngày giờ bắt đầu và ngày giờ nộp bài."
            });

            list.Add(new TestCase {
                Id = "TC-59", Sprint = "Sprint 2", Module = "Chấm điểm & Lưu kết quả thi (TASK-207)", TaskId = "TASK-207",
                Scenario = "Lưu chi tiết câu trả lời của thí sinh cho từng câu hỏi",
                Preconditions = "Đề thi có 30 câu hỏi.",
                TestData = "Tập hợp 30 câu trả lời của thí sinh",
                Steps = "1. Nộp bài thi.\n2. Kiểm tra dữ liệu chi tiết của bài thi vừa nộp.",
                Expected = "Lưu đầy đủ chi tiết cả 30 câu hỏi, ghi nhận rõ từng câu thí sinh đã chọn phương án nào và câu đó đúng hay sai."
            });

            list.Add(new TestCase {
                Id = "TC-60", Sprint = "Sprint 2", Module = "Chấm điểm & Lưu kết quả thi (TASK-207)", TaskId = "TASK-207",
                Scenario = "Xem lại danh sách lịch sử các lần thi thử trước đó",
                Preconditions = "Học viên đã từng làm 3 bài thi thử trong hệ thống.",
                TestData = "Mở mục 'Lịch sử thi' trong phần Thi thử",
                Steps = "1. Chuyển sang xem danh sách lịch sử thi.\n2. Đối chiếu số điểm, ngày thi và số câu đúng.",
                Expected = "Hiển thị đầy đủ cả 3 lần thi với điểm số, thời gian và số câu đúng, sắp xếp theo lần thi mới nhất lên đầu."
            });

            // TASK-208: Màn hình Xem lại bài thi (Review Mode)
            list.Add(new TestCase {
                Id = "TC-61", Sprint = "Sprint 2", Module = "Xem lại bài thi chi tiết (TASK-208)", TaskId = "TASK-208",
                Scenario = "Mở chế độ xem lại chi tiết từng câu hỏi của bài thi đã nộp",
                Preconditions = "Học viên vừa hoàn thành bài thi và bấm nút 'Xem lại bài thi'.",
                TestData = "Bài thi vừa hoàn thành",
                Steps = "1. Nhấn nút 'Xem lại bài thi' tại màn hình kết quả điểm số.\n2. Quan sát giao diện xem lại bài thi.",
                Expected = "Mở ra toàn bộ danh sách câu hỏi, đoạn văn đọc hiểu và các phương án trả lời theo đúng thứ tự lúc thí sinh làm bài thi."
            });

            list.Add(new TestCase {
                Id = "TC-62", Sprint = "Sprint 2", Module = "Xem lại bài thi chi tiết (TASK-208)", TaskId = "TASK-208",
                Scenario = "Đối chiếu trực quan đáp án thí sinh chọn với đáp án chuẩn và xem giải thích",
                Preconditions = "Đang ở chế độ xem lại bài thi.",
                TestData = "Câu 1: Thí sinh chọn đúng; Câu 2: Thí sinh chọn sai",
                Steps = "1. Xem câu 1 (chọn đúng).\n2. Xem câu 2 (chọn sai).\n3. Đọc khung giải thích chi tiết đáp án bên dưới.",
                Expected = "Hiển thị rõ ràng: Đáp án thí sinh chọn đúng được viền xanh lá; đáp án chọn sai được viền đỏ và hệ thống làm nổi bật đáp án đúng màu xanh. Phía dưới có khung giải thích ngữ pháp chi tiết."
            });

            list.Add(new TestCase {
                Id = "TC-63", Sprint = "Sprint 2", Module = "Xem lại bài thi chi tiết (TASK-208)", TaskId = "TASK-208",
                Scenario = "Thể hiện rõ ràng trạng thái đối với các câu hỏi thí sinh bỏ trống không làm",
                Preconditions = "Bài thi có câu hỏi thí sinh không chọn bất kỳ đáp án nào.",
                TestData = "Câu hỏi bị bỏ trống",
                Steps = "1. Điều hướng đến câu hỏi đã bị bỏ trống trong chế độ xem lại.\n2. Quan sát cách hiển thị.",
                Expected = "Hiển thị thông báo: 'Bạn đã bỏ trống câu hỏi này'. Đồng thời hệ thống vẫn làm nổi bật đáp án đúng màu xanh kèm giải thích để người học ôn bài."
            });

            list.Add(new TestCase {
                Id = "TC-64", Sprint = "Sprint 2", Module = "Xem lại bài thi chi tiết (TASK-208)", TaskId = "TASK-208",
                Scenario = "Kết quả bài thi cũ không bị thay đổi khi ngân hàng câu hỏi bị chỉnh sửa",
                Preconditions = "Đã thi xong bài thi trước đó; giảng viên vào ngân hàng câu hỏi sửa lại nội dung câu hỏi.",
                TestData = "Nội dung câu hỏi gốc trong ngân hàng bị sửa đổi",
                Steps = "1. Học viên mở lại kết quả bài thi cũ để xem lại.\n2. Kiểm tra nội dung câu hỏi hiển thị.",
                Expected = "Bài thi cũ vẫn giữ nguyên nội dung câu hỏi tại thời điểm thi, không bị sai lệch dữ liệu kết quả thi."
            });

            // TASK-209: Tích hợp Sprint 2
            list.Add(new TestCase {
                Id = "TC-65", Sprint = "Sprint 2", Module = "Kiểm thử liên hoàn Sprint 2 (TASK-209)", TaskId = "TASK-209",
                Scenario = "Kiểm tra chu trình thi thử hoàn chỉnh: Chọn đề, làm bài, nộp bài, xem điểm và xem lại bài",
                Preconditions = "Học viên đăng nhập tại màn hình chính.",
                TestData = "Chu trình thi thử từ đầu đến cuối",
                Steps = "1. Vào mục Thi thử, chọn đề thi TOEIC Mini-Test 30 câu.\n2. Làm bài trong 20 phút và nhấn Nộp bài.\n3. Xem bảng điểm tổng kết.\n4. Bấm 'Xem lại bài thi' để duyệt từng câu hỏi.",
                Expected = "Toàn bộ chu trình vận hành thông suốt: Đồng hồ đếm ngược chính xác, chấm điểm đúng, lưu kết quả an toàn, hiển thị bảng điểm rõ ràng và xem lại từng câu đối chiếu xanh đỏ chuẩn xác."
            });

            list.Add(new TestCase {
                Id = "TC-66", Sprint = "Sprint 2", Module = "Kiểm thử liên hoàn Sprint 2 (TASK-209)", TaskId = "TASK-209",
                Scenario = "Kiểm tra chu trình học tập: Đọc lý thuyết, nghe âm thanh, làm bài củng cố và nhận tiến độ 100%",
                Preconditions = "Học viên chọn một bài học mới trong lộ trình.",
                TestData = "Bài học có đủ phần lý thuyết, âm thanh và bài tập củng cố",
                Steps = "1. Mở bài học, đọc lý thuyết ngữ pháp.\n2. Bật âm thanh luyện nghe.\n3. Hoàn thành bài tập củng cố 5 câu trắc nghiệm.\n4. Bấm hoàn thành và quay lại Trang chủ kiểm tra.",
                Expected = "Hệ thống cộng dồn thời gian học thực tế vào hồ sơ, bài học được đánh dấu hoàn thành 100%, số bài đã hoàn thành trên Trang chủ tăng thêm 1 bài."
            });

            return list;
        }

        public static List<TestCase> GetE2ETestCases()
        {
            var list = new List<TestCase>();

            // E2E Toàn trình
            list.Add(new TestCase {
                Id = "TC-67", Sprint = "E2E", Module = "Kiểm thử toàn trình (End-to-End)", TaskId = "TASK-110, TASK-209",
                Scenario = "Kiểm tra toàn bộ hành trình của học viên mới từ Đăng ký, Khảo sát, Học tập đến Thi thử",
                Preconditions = "Ứng dụng mở tại màn hình đăng nhập ban đầu.",
                TestData = "Hành trình: Đăng ký -> Khảo sát đầu vào B1 -> Học bài nghe -> Thi thử",
                Steps = "1. Đăng ký tài khoản học viên mới.\n2. Đăng nhập, hệ thống tự đưa vào làm bài khảo sát đầu vào 20 câu.\n3. Nộp bài, nhận kết quả xếp lớp B1 và mở khóa màn hình chính.\n4. Vào 'Lộ trình học', hoàn thành 1 bài nghe và ghi nhận tiến độ 100%.\n5. Vào 'Thi thử', hoàn thành 1 đề thi thử và xem lại bài thi.\n6. Đăng xuất an toàn.",
                Expected = "Toàn bộ hành trình người dùng diễn ra liền mạch, dữ liệu đồng bộ chính xác trên toàn hệ thống, không phát sinh lỗi gián đoạn."
            });

            list.Add(new TestCase {
                Id = "TC-68", Sprint = "E2E", Module = "Kiểm thử toàn trình (End-to-End)", TaskId = "TASK-105, TASK-204",
                Scenario = "Bảo mật thông tin: Không bị lẫn dữ liệu khi đăng xuất và đổi tài khoản khác",
                Preconditions = "Có 2 tài khoản học viên khác nhau (Học viên A trình độ A2 và Học viên B trình độ B2).",
                TestData = "Đăng nhập tài khoản A -> Học 1 bài -> Đăng xuất -> Đăng nhập tài khoản B",
                Steps = "1. Đăng nhập tài khoản A, xem thông tin và tiến độ.\n2. Nhấn 'Đăng xuất'.\n3. Đăng nhập ngay bằng tài khoản B.\n4. Quan sát thông tin hiển thị trên Trang chủ của tài khoản B.",
                Expected = "Hệ thống xóa sạch dữ liệu phiên của tài khoản A, hiển thị chính xác tên, trình độ B2 và các bài học của tài khoản B, hoàn toàn không bị lộ thông tin của người dùng trước."
            });

            return list;
        }

        public static List<TraceItem> GetTraceabilityItems()
        {
            var list = new List<TraceItem>();

            list.Add(new TraceItem { Code = "TASK-101", Name = "Khởi tạo cơ sở dữ liệu & dữ liệu mẫu", AcceptanceCriteria = "Cài đặt CSDL, kiểm tra toàn vẹn, nạp dữ liệu mẫu ban đầu, xử lý mất kết nối", TestCaseIds = "TC-01, TC-02, TC-03, TC-04", Note = "Bao phủ đầy đủ 4 trường hợp" });
            list.Add(new TraceItem { Code = "TASK-102", Name = "Khởi động ứng dụng & kết nối", AcceptanceCriteria = "Khởi chạy ứng dụng nhanh chóng, kết nối ổn định, hiển thị màn hình đầu tiên", TestCaseIds = "TC-05", Note = "Kiểm tra khởi chạy ứng dụng" });
            list.Add(new TraceItem { Code = "TASK-104", Name = "Đăng ký & Đăng nhập tài khoản", AcceptanceCriteria = "Đăng ký thành công, bắt lỗi nhập sai, đăng nhập đúng, đăng nhập sai, chuyển đổi tab", TestCaseIds = "TC-06, TC-07, TC-08, TC-09, TC-10", Note = "Gộp các trường hợp nhập sai vào TC-07" });
            list.Add(new TraceItem { Code = "TASK-105", Name = "Bảo mật & Phân quyền học viên", AcceptanceCriteria = "Mã hóa mật khẩu an toàn, chặn tài khoản bị khóa, đăng xuất an toàn, phân quyền", TestCaseIds = "TC-11, TC-12, TC-13, TC-14", Note = "Kiểm tra các yêu cầu bảo mật" });
            list.Add(new TraceItem { Code = "TASK-106", Name = "Khung giao diện chính sinh viên", AcceptanceCriteria = "Hiển thị menu, avatar viết tắt, chuyển đổi trang, co giãn cửa sổ", TestCaseIds = "TC-15, TC-16, TC-17", Note = "Gộp chuyển trang và highlight vào TC-16" });
            list.Add(new TraceItem { Code = "TASK-107", Name = "Quy trình cho người dùng mới (Onboarding)", AcceptanceCriteria = "Bắt buộc thi đầu vào khi đăng nhập lần đầu, mở khóa menu sau khi thi, tài khoản cũ vào thẳng", TestCaseIds = "TC-18, TC-19, TC-20, TC-21", Note = "Kiểm tra luồng người dùng mới" });
            list.Add(new TraceItem { Code = "TASK-108", Name = "Giao diện Khảo sát năng lực đầu vào", AcceptanceCriteria = "Tải 20 câu hỏi, chọn/đổi đáp án, chuyển câu, cảnh báo chưa làm hết, giấu đáp án", TestCaseIds = "TC-22, TC-23, TC-24, TC-25", Note = "Gộp thao tác chọn đáp án vào TC-23" });
            list.Add(new TraceItem { Code = "TASK-109", Name = "Đánh giá trình độ & Xếp loại CEFR", AcceptanceCriteria = "Tính số câu đúng, xếp loại A1-C1 theo thang điểm, lưu hồ sơ học viên, nộp bài 0đ", TestCaseIds = "TC-26, TC-27, TC-28, TC-29", Note = "Gộp các mức điểm ranh giới vào TC-27" });
            list.Add(new TraceItem { Code = "TASK-110", Name = "Kiểm thử liên hoàn Sprint 1", AcceptanceCriteria = "Chu trình người dùng mới liên hoàn; đăng xuất đăng nhập lại bảo toàn hồ sơ", TestCaseIds = "TC-30, TC-31", Note = "Kiểm tra liên hoàn quy trình đầu vào" });

            list.Add(new TraceItem { Code = "TASK-201", Name = "Trang chủ học viên & Thống kê", AcceptanceCriteria = "Lời chào cá nhân, huy hiệu trình độ, 3 thẻ thống kê, bài học gần đây, nút lối tắt", TestCaseIds = "TC-32, TC-33, TC-34, TC-35", Note = "Gộp hiển thị bài gần đây vào TC-34" });
            list.Add(new TraceItem { Code = "TASK-202", Name = "Học tập theo Lộ trình", AcceptanceCriteria = "Danh mục bài học theo chủ đề, lọc kỹ năng cốt lõi, đọc bài học chi tiết, giữ bộ lọc", TestCaseIds = "TC-36, TC-37, TC-38, TC-39", Note = "Kiểm tra chức năng lộ trình học" });
            list.Add(new TraceItem { Code = "TASK-203", Name = "Trình phát âm thanh bài nghe", AcceptanceCriteria = "Phát, tạm dừng, tua, đổi tốc độ, tự động đọc khi thiếu file âm thanh, chống phát đè", TestCaseIds = "TC-40, TC-41, TC-42", Note = "Gộp các nút điều khiển vào TC-40" });
            list.Add(new TraceItem { Code = "TASK-204", Name = "Ghi nhận Thời gian & Tiến độ", AcceptanceCriteria = "Cộng dồn thời gian học thực tế, hoàn thành 100% khi xong bài tập, chống thoát nhanh", TestCaseIds = "TC-43, TC-44, TC-45, TC-46", Note = "Gộp chống gian lận thời gian vào TC-46" });
            list.Add(new TraceItem { Code = "TASK-205", Name = "Luyện tập theo Kỹ năng", AcceptanceCriteria = "Tải câu hỏi luyện tập, báo đúng sai tức thì, giải thích đáp án, Flashcard, Dictation", TestCaseIds = "TC-47, TC-48, TC-49, TC-50", Note = "Gộp báo đúng sai và giải thích vào TC-48" });
            list.Add(new TraceItem { Code = "TASK-206", Name = "Động cơ Thi thử Chuẩn hóa", AcceptanceCriteria = "Danh mục đề thi, phòng thi, cắm cờ câu khó, đồng hồ đếm ngược, nộp bài, hết giờ tự nộp", TestCaseIds = "TC-51, TC-52, TC-53, TC-54, TC-55, TC-56", Note = "Gộp thao tác phòng thi vào TC-52" });
            list.Add(new TraceItem { Code = "TASK-207", Name = "Chấm điểm & Lưu lịch sử thi", AcceptanceCriteria = "Chấm điểm thang 10.0, lưu kết quả tổng quan, lưu chi tiết từng câu, xem lại lịch sử thi", TestCaseIds = "TC-57, TC-58, TC-59, TC-60", Note = "Gộp các mức điểm vào TC-57" });
            list.Add(new TraceItem { Code = "TASK-208", Name = "Xem lại bài thi chi tiết (Review Mode)", AcceptanceCriteria = "Xem lại câu hỏi, đối chiếu xanh đỏ kèm giải thích, thể hiện câu bỏ trống, cố định kết quả", TestCaseIds = "TC-61, TC-62, TC-63, TC-64", Note = "Gộp đối chiếu và giải thích vào TC-62" });
            list.Add(new TraceItem { Code = "TASK-209", Name = "Kiểm thử liên hoàn Sprint 2", AcceptanceCriteria = "Chu trình thi thử hoàn chỉnh; chu trình học tập kết hợp bài tập củng cố", TestCaseIds = "TC-65, TC-66", Note = "Kiểm tra liên hoàn Sprint 2" });
            list.Add(new TraceItem { Code = "E2E", Name = "Kiểm thử toàn trình (End-to-End)", AcceptanceCriteria = "Toàn bộ hành trình trải nghiệm học viên; bảo mật tài khoản khi đổi người dùng", TestCaseIds = "TC-67, TC-68", Note = "Kiểm tra toàn trình trải nghiệm" });

            return list;
        }
    }
}
