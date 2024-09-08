<!DOCTYPE qgis PUBLIC 'http://mrcc.com/qgis.dtd' 'SYSTEM'>
<qgis readOnly="0" labelsEnabled="0" version="3.28.8-Firenze" styleCategories="LayerConfiguration|Symbology|Labeling">
  <flags>
    <Identifiable>1</Identifiable>
    <Removable>1</Removable>
    <Searchable>1</Searchable>
    <Private>0</Private>
  </flags>
  <renderer-v2 enableorderby="0" toleranceUnitScale="3x:0,0,0,0,0,0" referencescale="-1" tolerance="3" toleranceUnit="MM" type="pointCluster" forceraster="0" symbollevels="0">
    <renderer-v2 enableorderby="0" referencescale="-1" type="RuleRenderer" forceraster="0" symbollevels="0">
      <rules key="{95a11461-b853-4a26-9b8c-4e067a3ec7b5}">
        <rule label="Fire" key="{ab07a800-3d4d-415f-9138-102cdb87319a}" symbol="0" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; = 'Fire'"/>
        <rule label="Health" key="{dc4a2388-b378-4678-a570-8287063e62f6}" symbol="1" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; = 'Health'"/>
        <rule label="Education" key="{0b71e9d6-d851-4dd0-b645-69eba018b9c5}" symbol="2" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; ~ 'Education'"/>
        <rule label="Government" key="{e3646b7b-94e7-4958-848a-801f12763215}" symbol="3" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; = 'Admin'"/>
        <rule label="Police" key="{fce03c4b-dd2a-4b5c-b04b-3333828f4ca0}" symbol="4" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; = 'Police'"/>
        <rule label="Post" key="{5d5837b8-ec09-48ff-97a2-1907bad8155d}" symbol="5" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; = 'Post'"/>
        <rule label="Power" key="{8c3603dc-0306-46f2-935a-2365465a1de6}" symbol="6" scalemindenom="3500" scalemaxdenom="20000" filter="(&quot;Category&quot; ~ 'Power') AND NOT (&quot;Category&quot; ~ 'Waste')"/>
        <rule label="Sewage &amp; Water" key="{a80b9a3e-99df-4059-b6c4-6dccb7e5aecb}" symbol="7" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; ~ 'Sewage|Water'"/>
        <rule label="Waste" key="{47134359-2cd0-44cd-9fa8-a9c21d8873bd}" symbol="8" scalemindenom="3500" scalemaxdenom="20000" filter="&quot;Category&quot; ~ 'Waste'"/>
        <rule label="Helipad" key="{e7faee1e-74ac-491b-9632-3b93b689a678}" symbol="9" scalemindenom="1" scalemaxdenom="20000" filter="&quot;Category&quot; =  'Helipad'"/>
        <rule label="Utility Poles" key="{df3f6e7d-ae2d-4376-a310-35feab84271d}" symbol="10" scalemindenom="1" scalemaxdenom="40000" filter="&quot;Category&quot; =  'UtilityPole'"/>
        <rule label="Utility Pylons" key="{8672a9bc-97e0-44bd-a7b3-5edd6fa6831e}" symbol="11" scalemindenom="1" scalemaxdenom="40000" filter="&quot;Category&quot; =  'UtilityPylon'"/>
      </rules>
      <symbols>
        <symbol frame_rate="10" name="0" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="1">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="255,90,90,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="emergency/amenity=fire_station.svg" type="QString"/>
              <Option name="offset" value="-0.10000000000000001,-0.10000000000000001" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="3" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="1" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="cap_style" value="square" type="QString"/>
              <Option name="color" value="255,90,90,255" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="name" value="cross_fill" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,0" type="QString"/>
              <Option name="outline_style" value="solid" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="2.4" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="10" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="cap_style" value="square" type="QString"/>
              <Option name="color" value="231,231,231,255" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="name" value="circle" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="100,100,100,255" type="QString"/>
              <Option name="outline_style" value="solid" type="QString"/>
              <Option name="outline_width" value="0.1" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="1.5" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="cap_style" value="square" type="QString"/>
              <Option name="color" value="255,0,0,255" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="name" value="cross2" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="100,100,100,255" type="QString"/>
              <Option name="outline_style" value="solid" type="QString"/>
              <Option name="outline_width" value="0.1" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="1" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="11" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="cap_style" value="square" type="QString"/>
              <Option name="color" value="231,231,231,255" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="name" value="rounded_square" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="100,100,100,255" type="QString"/>
              <Option name="outline_style" value="solid" type="QString"/>
              <Option name="outline_width" value="0.1" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="10" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="cap_style" value="square" type="QString"/>
              <Option name="color" value="255,0,0,255" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="name" value="cross2" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="100,100,100,255" type="QString"/>
              <Option name="outline_style" value="solid" type="QString"/>
              <Option name="outline_width" value="0.1" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="10" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="2" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="200,160,100,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="amenity/amenity_library.svg" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,0" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="3" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="3" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="200,160,100,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="tourist/tourist_museum.svg" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,0" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="3" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="4" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="200,160,100,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="amenity/amenity_police.svg" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,0" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="3" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="5" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="200,160,100,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="amenity/amenity_post_box.svg" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,0" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="3.6" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="6" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="FontMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="chr" value="⚡" type="QString"/>
              <Option name="color" value="50,150,150,255" type="QString"/>
              <Option name="font" value="Segoe UI" type="QString"/>
              <Option name="font_style" value="Black" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="miter" type="QString"/>
              <Option name="offset" value="0,-0.40000000000000008" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="size" value="3" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="7" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="FontMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="chr" value="💧" type="QString"/>
              <Option name="color" value="50,150,150,255" type="QString"/>
              <Option name="font" value="Segoe UI" type="QString"/>
              <Option name="font_style" value="Black" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="miter" type="QString"/>
              <Option name="offset" value="0,-0.29999999999999999" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="size" value="2.75" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="8" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="SvgMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="color" value="50,150,150,255" type="QString"/>
              <Option name="fixedAspectRatio" value="0" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="name" value="amenity/amenity_recycling.svg" type="QString"/>
              <Option name="offset" value="0,0" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="parameters"/>
              <Option name="scale_method" value="diameter" type="QString"/>
              <Option name="size" value="2.75" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="MM" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol frame_rate="10" name="9" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </data_defined_properties>
          <layer enabled="1" locked="0" class="FontMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="chr" value="◯" type="QString"/>
              <Option name="color" value="140,140,140,255" type="QString"/>
              <Option name="font" value="Segoe UI" type="QString"/>
              <Option name="font_style" value="Semibold" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="offset" value="0,-0.80000000000000093" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="size" value="10" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer enabled="1" locked="0" class="FontMarker" pass="0">
            <Option type="Map">
              <Option name="angle" value="0" type="QString"/>
              <Option name="chr" value="H" type="QString"/>
              <Option name="color" value="140,140,140,255" type="QString"/>
              <Option name="font" value="Segoe UI" type="QString"/>
              <Option name="font_style" value="Semibold" type="QString"/>
              <Option name="horizontal_anchor_point" value="1" type="QString"/>
              <Option name="joinstyle" value="bevel" type="QString"/>
              <Option name="offset" value="0,-0.60000000000000098" type="QString"/>
              <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offset_unit" value="MM" type="QString"/>
              <Option name="outline_color" value="35,35,35,255" type="QString"/>
              <Option name="outline_width" value="0" type="QString"/>
              <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="outline_width_unit" value="MM" type="QString"/>
              <Option name="size" value="7" type="QString"/>
              <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="size_unit" value="RenderMetersInMapUnits" type="QString"/>
              <Option name="vertical_anchor_point" value="1" type="QString"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
      </symbols>
    </renderer-v2>
    <symbol frame_rate="10" name="centerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
      <data_defined_properties>
        <Option type="Map">
          <Option name="name" value="" type="QString"/>
          <Option name="properties"/>
          <Option name="type" value="collection" type="QString"/>
        </Option>
      </data_defined_properties>
      <layer enabled="0" locked="0" class="SimpleMarker" pass="0">
        <Option type="Map">
          <Option name="angle" value="0" type="QString"/>
          <Option name="cap_style" value="square" type="QString"/>
          <Option name="color" value="245,75,80,0" type="QString"/>
          <Option name="horizontal_anchor_point" value="1" type="QString"/>
          <Option name="joinstyle" value="bevel" type="QString"/>
          <Option name="name" value="circle" type="QString"/>
          <Option name="offset" value="0,0" type="QString"/>
          <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
          <Option name="offset_unit" value="MM" type="QString"/>
          <Option name="outline_color" value="35,35,35,0" type="QString"/>
          <Option name="outline_style" value="solid" type="QString"/>
          <Option name="outline_width" value="0" type="QString"/>
          <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
          <Option name="outline_width_unit" value="MM" type="QString"/>
          <Option name="scale_method" value="diameter" type="QString"/>
          <Option name="size" value="1" type="QString"/>
          <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
          <Option name="size_unit" value="MM" type="QString"/>
          <Option name="vertical_anchor_point" value="1" type="QString"/>
        </Option>
        <data_defined_properties>
          <Option type="Map">
            <Option name="name" value="" type="QString"/>
            <Option name="properties"/>
            <Option name="type" value="collection" type="QString"/>
          </Option>
        </data_defined_properties>
      </layer>
    </symbol>
  </renderer-v2>
  <labeling type="rule-based">
    <rules key="{04342332-3ca6-4eba-bef2-631ace81c93e}">
      <rule key="{ff43ff18-d1b8-4625-897f-8dbbfc2bedb2}" scalemindenom="1" description="Park" scalemaxdenom="7500" filter="&quot;Category&quot; ~ 'Attraction|MortuaryCemetery|Park(?!ing)'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="80,180,60,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Aa" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if(@id % 2 = 0, 5, 3)" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{5d560d3a-0ae5-43a2-8c4e-79fdc58c31f5}" scalemindenom="1" description="Emergency" scalemaxdenom="20000" filter="&quot;Category&quot; ~ 'Fire|Health'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="239,144,144,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Em" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if(@id % 2 = 0, 5, 3)" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{cc0f268d-0f91-4f34-99fb-643594d82e80}" scalemindenom="1" description="Government" scalemaxdenom="15000" filter="&quot;Category&quot; ~ 'Admin|Education|Police|Post(?!Box)'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="162,137,88,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Gv" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="0" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="AutoWrapLength" type="Map">
                  <Option name="active" value="false" type="bool"/>
                  <Option name="type" value="1" type="int"/>
                  <Option name="val" value="" type="QString"/>
                </Option>
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if(@id % 2 = 0, 5, 3)" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{16f9f6de-848a-4280-a029-b47a11322805}" scalemindenom="1" description="Bus / Tram Stops" scalemaxdenom="2500" filter="&quot;Category&quot; ~ 'Stop(?=Bus|Tram)'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="20,145,230,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="8" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Tr" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="0" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="2.5" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="AutoWrapLength" type="Map">
                  <Option name="active" value="false" type="bool"/>
                  <Option name="type" value="1" type="int"/>
                  <Option name="val" value="" type="QString"/>
                </Option>
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if(@id % 2 = 0, 5, 3)" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{0e335871-35f0-45b2-a683-36b0ecb26fa2}" scalemindenom="1" description="Transportation" scalemaxdenom="30000" filter="&quot;Category&quot; ~ 'Building(Bus|PassengerShip|PassengerTrain|Subway|Tram)' AND NOT &quot;Category&quot; ~ 'Airplane'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="20,145,230,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Tr" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="-3" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="LabelDistance" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="5" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{d5667ebf-487e-4d51-b4f6-683fbcfd9852}" scalemindenom="1" description="Stores" scalemaxdenom="4000" filter="&quot;Category&quot; ~ '(?:Store(?:Bank|Bar|Beverage|ConvenienceStore|Food|GasStation|Hotel|Restaurant)|Parking)'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="0,0,0,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="St" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="Color" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if(&quot;Category&quot; ~ 'Store(Bank|ConvenienceStore|Hotel)', '#ff78c8', if(&quot;Category&quot; ~ 'Store(Bar|Beverage|Food|Restaurant)', '#c864ff', if(&quot;Category&quot; ~ 'StoreGasStation|Parking', '#8296f0', '#ff78c8')))" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="5" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{9cd4a3f2-b514-4a93-be8e-20b1d7769519}" scalemindenom="1" description="Utility" scalemaxdenom="7500" filter="&quot;Category&quot; ~ 'Power|Sewage|Waste|Water'">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="50,150,150,255" fontWeight="75" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="9" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="1" legendString="Ut" fontSizeUnit="Point" fontKerning="1" namedStyle="Bold" textOrientation="horizontal" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="1" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="-3" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="5" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" placementFlags="10" priority="7" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="3" offsetType="0" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="6" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties" type="Map">
                <Option name="LabelDistance" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="if ((&quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingBus((?!Airplane|Subway|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway|Train).)*BuildingPassengerShip((?!Airplane|Subway|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingBus((?!Airplane|Ship|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Subway).)*BuildingPassengerTrain((?!Airplane|Ship|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingBus((?!Airplane|Ship|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship|Train).)*BuildingSubway((?!Airplane|Ship|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Subway).)*BuildingPassengerShip((?!Airplane|Bus|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerTrain((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingPassengerShip((?!Airplane|Bus|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Train).)*BuildingSubway((?!Airplane|Bus|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingPassengerTrain((?!Airplane|Bus|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus|Ship).)*BuildingSubway((?!Airplane|Bus|Ship).)*$'), 5, if ((&quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingBus((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerShip((?!Airplane|Subway).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Subway).)*BuildingPassengerTrain((?!Airplane|Subway).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingBus((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingPassengerShip((?!Airplane|Train).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Train).)*BuildingSubway((?!Airplane|Train).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingBus((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingPassengerTrain((?!Airplane|Ship).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Ship).)*BuildingSubway((?!Airplane|Ship).)*$') OR (&quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerShip((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingPassengerTrain((?!Airplane|Bus).)*$' AND &quot;Category&quot; ~ '^((?!Airplane|Bus).)*BuildingSubway((?!Airplane|Bus).)*$'), 7, 3))" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="OffsetQuad" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="5" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
                <Option name="PredefinedPositionOrder" type="Map">
                  <Option name="active" value="true" type="bool"/>
                  <Option name="expression" value="'R,L'" type="QString"/>
                  <Option name="type" value="3" type="int"/>
                </Option>
              </Option>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{2afb3ea8-cdac-47cf-a3c2-3095ec5c3769}" filter="ELSE">
        <settings calloutType="simple">
          <text-style fontLetterSpacing="0" blendMode="0" allowHtml="0" capitalization="0" isExpression="1" fontItalic="0" textColor="255,255,255,255" fontWeight="50" fontWordSpacing="0" multilineHeight="1" forcedItalic="0" fontSizeMapUnitScale="3x:0,0,0,0,0,0" fontSize="1" fontStrikeout="0" previewBkgrdColor="255,255,255,255" useSubstitutions="0" textOpacity="0" legendString="Aa" fontSizeUnit="Point" fontKerning="1" namedStyle="Regular" textOrientation="horizontal" fieldName="0" forcedBold="0" fontUnderline="0" multilineHeightUnit="Percentage" fontFamily="Microsoft JhengHei UI">
            <families/>
            <text-buffer bufferJoinStyle="128" bufferDraw="0" bufferNoFill="1" bufferBlendMode="0" bufferColor="250,250,250,255" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferOpacity="0.69999999999999996" bufferSize="0.5" bufferSizeUnits="MM"/>
            <text-mask maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskSizeUnits="MM" maskEnabled="0" maskSize="0" maskJoinStyle="128" maskOpacity="1" maskedSymbolLayers=""/>
            <background shapeOffsetY="0" shapeRadiiUnit="Point" shapeSizeType="0" shapeOpacity="1" shapeRadiiY="0" shapeBorderWidthUnit="Point" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeType="0" shapeRotation="0" shapeSizeX="0" shapeBorderColor="128,128,128,255" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiX="0" shapeJoinStyle="64" shapeOffsetX="0" shapeSizeY="0" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeOffsetUnit="Point" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeBorderWidth="0" shapeSizeUnit="Point" shapeSVGFile="" shapeDraw="0" shapeFillColor="255,255,255,255">
              <symbol frame_rate="10" name="markerSymbol" alpha="1" is_animated="0" type="marker" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleMarker" pass="0">
                  <Option type="Map">
                    <Option name="angle" value="0" type="QString"/>
                    <Option name="cap_style" value="square" type="QString"/>
                    <Option name="color" value="152,125,183,255" type="QString"/>
                    <Option name="horizontal_anchor_point" value="1" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="name" value="circle" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="35,35,35,255" type="QString"/>
                    <Option name="outline_style" value="solid" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="outline_width_unit" value="MM" type="QString"/>
                    <Option name="scale_method" value="diameter" type="QString"/>
                    <Option name="size" value="2" type="QString"/>
                    <Option name="size_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="size_unit" value="MM" type="QString"/>
                    <Option name="vertical_anchor_point" value="1" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol frame_rate="10" name="fillSymbol" alpha="1" is_animated="0" type="fill" clip_to_extent="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option name="name" value="" type="QString"/>
                    <Option name="properties"/>
                    <Option name="type" value="collection" type="QString"/>
                  </Option>
                </data_defined_properties>
                <layer enabled="1" locked="0" class="SimpleFill" pass="0">
                  <Option type="Map">
                    <Option name="border_width_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="color" value="255,255,255,255" type="QString"/>
                    <Option name="joinstyle" value="bevel" type="QString"/>
                    <Option name="offset" value="0,0" type="QString"/>
                    <Option name="offset_map_unit_scale" value="3x:0,0,0,0,0,0" type="QString"/>
                    <Option name="offset_unit" value="MM" type="QString"/>
                    <Option name="outline_color" value="128,128,128,255" type="QString"/>
                    <Option name="outline_style" value="no" type="QString"/>
                    <Option name="outline_width" value="0" type="QString"/>
                    <Option name="outline_width_unit" value="Point" type="QString"/>
                    <Option name="style" value="solid" type="QString"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option name="name" value="" type="QString"/>
                      <Option name="properties"/>
                      <Option name="type" value="collection" type="QString"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadius="1.5" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowScale="100" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOffsetDist="1" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOpacity="0.69999999999999996" shadowUnder="0" shadowOffsetGlobal="1" shadowRadiusUnit="MM" shadowDraw="0" shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowBlendMode="6"/>
            <dd_properties>
              <Option type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format leftDirectionSymbol="&lt;" decimals="3" addDirectionSymbol="0" reverseDirectionSymbol="0" placeDirectionSymbol="0" multilineAlign="3" autoWrapLength="12" wrapChar="" rightDirectionSymbol=">" formatNumbers="0" useMaxLineLengthForAutoWrap="1" plussign="0"/>
          <placement preserveRotation="1" overrunDistance="0" lineAnchorPercent="0.5" yOffset="0" geometryGeneratorType="PointGeometry" geometryGeneratorEnabled="0" repeatDistanceUnits="MM" rotationAngle="0" overlapHandling="PreventOverlap" offsetUnits="MM" lineAnchorTextPoint="FollowPlacement" centroidInside="0" quadOffset="4" maxCurvedCharAngleOut="-25" repeatDistance="0" allowDegraded="0" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" fitInPolygonOnly="0" lineAnchorClipping="0" distUnits="MM" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="0" placementFlags="10" priority="5" lineAnchorType="0" rotationUnit="AngleDegrees" centroidWhole="0" distMapUnitScale="3x:0,0,0,0,0,0" geometryGenerator="" xOffset="0" offsetType="1" polygonPlacementFlags="2" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" overrunDistanceUnit="MM" maxCurvedCharAngleIn="25" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" placement="1" layerType="PointGeometry"/>
          <rendering upsidedownLabels="0" minFeatureSize="0" maxNumLabels="2000" fontMaxPixelSize="10000" fontMinPixelSize="3" limitNumLabels="0" drawLabels="1" scaleMin="0" obstacle="1" fontLimitPixelSize="0" mergeLines="0" obstacleType="1" obstacleFactor="1.6000000000000001" labelPerPart="0" scaleMax="0" zIndex="0" scaleVisibility="0" unplacedVisibility="0"/>
          <dd_properties>
            <Option type="Map">
              <Option name="name" value="" type="QString"/>
              <Option name="properties"/>
              <Option name="type" value="collection" type="QString"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option name="anchorPoint" value="pole_of_inaccessibility" type="QString"/>
              <Option name="blendMode" value="0" type="int"/>
              <Option name="ddProperties" type="Map">
                <Option name="name" value="" type="QString"/>
                <Option name="properties"/>
                <Option name="type" value="collection" type="QString"/>
              </Option>
              <Option name="drawToAllParts" value="false" type="bool"/>
              <Option name="enabled" value="0" type="QString"/>
              <Option name="labelAnchorPoint" value="point_on_exterior" type="QString"/>
              <Option name="lineSymbol" value="&lt;symbol frame_rate=&quot;10&quot; name=&quot;symbol&quot; alpha=&quot;1&quot; is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot; pass=&quot;0&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;align_dash_pattern&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;capstyle&quot; value=&quot;square&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash&quot; value=&quot;5;2&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;customdash_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;dash_pattern_offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;draw_inside_polygon&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;joinstyle&quot; value=&quot;bevel&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_color&quot; value=&quot;60,60,60,255&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_style&quot; value=&quot;solid&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width&quot; value=&quot;0.3&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;line_width_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;offset_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;ring_filter&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_end_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;trim_distance_start_unit&quot; value=&quot;MM&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;tweak_dash_pattern_on_corners&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;use_custom_dash&quot; value=&quot;0&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;width_map_unit_scale&quot; value=&quot;3x:0,0,0,0,0,0&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option name=&quot;name&quot; value=&quot;&quot; type=&quot;QString&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option name=&quot;type&quot; value=&quot;collection&quot; type=&quot;QString&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" type="QString"/>
              <Option name="minLength" value="0" type="double"/>
              <Option name="minLengthMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="minLengthUnit" value="MM" type="QString"/>
              <Option name="offsetFromAnchor" value="0" type="double"/>
              <Option name="offsetFromAnchorMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromAnchorUnit" value="MM" type="QString"/>
              <Option name="offsetFromLabel" value="0" type="double"/>
              <Option name="offsetFromLabelMapUnitScale" value="3x:0,0,0,0,0,0" type="QString"/>
              <Option name="offsetFromLabelUnit" value="MM" type="QString"/>
            </Option>
          </callout>
        </settings>
      </rule>
    </rules>
  </labeling>
  <blendMode>0</blendMode>
  <featureBlendMode>0</featureBlendMode>
  <previewExpression>"Category"</previewExpression>
  <layerGeometryType>0</layerGeometryType>
</qgis>
