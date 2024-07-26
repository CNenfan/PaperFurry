<?php
// 检查是否有数据提交
if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['data'])) {
    $receivedData = $_POST['data'];
    
    // 在这里处理接收到的数据，例如保存到数据库、文件等
    // 以下仅为示例逻辑，根据实际情况编写处理代码
    if ($receivedData) {
        // 假设数据处理成功
        echo "succeeded"; // 返回成功信息给Unity客户端
        // 可以根据需要添加更多处理逻辑
    } else {
        // 数据处理失败的情况
        http_response_code(400); // 设置HTTP响应码为400，表示客户端错误
        echo "数据处理失败，请检查发送的内容。";
    }
} else {
    // 如果不是POST请求或缺少'data'字段
    http_response_code(400);
    echo "无效的请求，请确保使用POST方法并包含'data'字段。";
}