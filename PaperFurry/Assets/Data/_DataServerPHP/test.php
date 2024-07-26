<?php
// 指定上传文件存放的目录
$uploadDir = 'uploads/';

// 获取上传的文件名（这里假设Unity上传时使用了正确的键名"data"和"key"）
$dataFile = $_FILES['data'];
$keyFile = $_FILES['key'];

// 验证文件是否上传成功
if ($dataFile['error'] === UPLOAD_ERR_OK && $keyFile['error'] === UPLOAD_ERR_OK) {
    // 构建保存的文件名
    $saveName = basename($dataFile['name'], '.save.rf'); // 移除后缀得到saveName
    $dataFilePath = $uploadDir . $saveName . '.save.rf';
    $keyFilePath = $uploadDir . $saveName . '.key.save.rf';

    // 尝试保存文件
    if (move_uploaded_file($dataFile['tmp_name'], $dataFilePath) && move_uploaded_file($keyFile['tmp_name'], $keyFilePath)) {
        echo json_encode(['status' => 'success', 'message' => 'file uploaded successfully']);
        exit;
    } else {
        echo json_encode(['status' => 'error', 'message' => 'file save failed']);
        exit;
    }
} else {
    echo json_encode(['status' => 'error', 'message' => 'file upload failed']);
    exit;
}