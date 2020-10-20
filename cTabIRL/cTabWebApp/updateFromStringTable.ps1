[xml]$resxEnglish = Get-Content $PSScriptRoot\SharedResource.resx
[xml]$resxFrench = Get-Content $PSScriptRoot\SharedResource.fr.resx
[xml]$stringtable = Get-Content $PSScriptRoot\..\..\@cTab\addons\core\stringtable.xml

foreach($node in $stringtable.selectNodes("Project/Package/Key"))
{
    $ID = $node.GetAttribute("ID")

    $match = $resxEnglish.SelectSingleNode("root/data[name='$ID']")
    if (!$match) 
    {
        $data = $resxEnglish.CreateElement("data")
        $data.SetAttribute("name", $ID)

        $value = $resxEnglish.CreateElement("value")
        $value.InnerText = $node.SelectSingleNode("English").InnerText

         $data.AppendChild($value)
        $resxEnglish.SelectSingleNode("root").AppendChild($data)
    }
    else
    {
        $match.SelectSingleNode("value").InnerText = $node.SelectSingleNode("English").InnerText
    }


    $match = $resxFrench.SelectSingleNode("root/data[name='$ID']")
    if (!$match) 
    {
        $data = $resxFrench.CreateElement("data")
        $data.SetAttribute("name", $ID)

        $value = $resxFrench.CreateElement("value")
        $value.InnerText = $node.SelectSingleNode("French").InnerText

        $data.AppendChild($value)
        $resxFrench.SelectSingleNode("root").AppendChild($data)
    }
    else
    {
        $match.SelectSingleNode("value").InnerText = $node.SelectSingleNode("French").InnerText
    }


}

$resxEnglish.Save("$PSScriptRoot\SharedResource.resx")
$resxFrench.Save("$PSScriptRoot\SharedResource.fr.resx")