$content = Get-Content 'C:\Users\admin\.gemini\antigravity-ide\brain\4ff7cae6-58ac-4398-b356-ea12e7631c43\.system_generated\logs\transcript_full.jsonl'
foreach ($item in $content) {
    if ($item.Contains('BẮT BUỘC CHỈ SỬ DỤNG 5 CỘT')) {
        $json = ConvertFrom-Json $item
        Set-Content -Path 'd:\TDC\HK5\CDPTUD\EAI\prompt9_full.txt' -Value $json.content -Encoding utf8
        break
    }
}
