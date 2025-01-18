function Update-ResxFiles {
    param (
        [xml]$resxEnglish,
        [xml]$resxFrench,
        [xml]$stringtable
    )

    foreach($node in $stringtable.selectNodes("Project/Package/Key")) {
        $ID = $node.GetAttribute("ID")

        $match = $resxEnglish.SelectSingleNode("root/data[@name='"+$ID+"']")
        if ($match -eq $null) {
            $data = $resxEnglish.CreateElement("data")
            $data.SetAttribute("name", $ID)

            $value = $resxEnglish.CreateElement("value")
            $value.InnerText = $node.SelectSingleNode("English").InnerText

            $data.AppendChild($value)
            $resxEnglish.SelectSingleNode("root").AppendChild($data)
        } else {
            $match.SelectSingleNode("value").InnerText = $node.SelectSingleNode("English").InnerText
        }

        $match = $resxFrench.SelectSingleNode("root/data[@name='"+$ID+"']")
        if ($match -eq $null) {
            $data = $resxFrench.CreateElement("data")
            $data.SetAttribute("name", $ID)

            $value = $resxFrench.CreateElement("value")
            $value.InnerText = $node.SelectSingleNode("French").InnerText

            $data.AppendChild($value)
            $resxFrench.SelectSingleNode("root").AppendChild($data)
        } else {
            $match.SelectSingleNode("value").InnerText = $node.SelectSingleNode("French").InnerText
        }
    }

}

[xml]$resxEnglish = Get-Content $PSScriptRoot\SharedResource.resx -Encoding UTF8
[xml]$resxFrench = Get-Content $PSScriptRoot\SharedResource.fr.resx -Encoding UTF8
[xml]$stringtable1 = Get-Content $PSScriptRoot\..\..\@cTab\addons\core\stringtable.xml -Encoding UTF8
[xml]$stringtable2 = Get-Content $PSScriptRoot\..\..\@cTab\addons\messaging\stringtable.xml -Encoding UTF8

Update-ResxFiles -resxEnglish $resxEnglish -resxFrench $resxFrench -stringtable $stringtable1
Update-ResxFiles -resxEnglish $resxEnglish -resxFrench $resxFrench -stringtable $stringtable2


$resxEnglish.Save("$PSScriptRoot\SharedResource.resx")
$resxFrench.Save("$PSScriptRoot\SharedResource.fr.resx")