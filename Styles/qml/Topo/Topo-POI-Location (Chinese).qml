<!DOCTYPE qgis PUBLIC 'http://mrcc.com/qgis.dtd' 'SYSTEM'>
<qgis version="3.28.8-Firenze" labelsEnabled="1" styleCategories="LayerConfiguration|Symbology|Labeling" readOnly="0">
  <flags>
    <Identifiable>1</Identifiable>
    <Removable>1</Removable>
    <Searchable>1</Searchable>
    <Private>0</Private>
  </flags>
  <renderer-v2 enableorderby="0" type="pointCluster" tolerance="4" forceraster="0" symbollevels="0" toleranceUnitScale="3x:0,0,0,0,0,0" toleranceUnit="MM" referencescale="-1">
    <renderer-v2 enableorderby="0" type="RuleRenderer" forceraster="0" symbollevels="0" referencescale="-1">
      <rules key="{95a11461-b853-4a26-9b8c-4e067a3ec7b5}">
        <rule label="Emergency" symbol="0" scalemaxdenom="100000" key="{b4f47d9b-a45a-4d4f-86d1-92938e7ac6bd}" scalemindenom="20000" filter="&quot;Category&quot; ~ 'Fire|Health'"/>
        <rule label="Fire" symbol="1" scalemaxdenom="20000" key="{d00e65d9-1dde-447f-8671-3ef335143a9e}" scalemindenom="1" filter="&quot;Category&quot; = 'Fire'"/>
        <rule label="Health" symbol="2" scalemaxdenom="20000" key="{436d1e10-ed3d-4b93-b7da-0300dff3d937}" scalemindenom="1" filter="&quot;Category&quot; = 'Health'"/>
        <rule label="Public" symbol="3" scalemaxdenom="100000" key="{873f27af-d6a0-4c5a-a138-6f7c6d30139d}" scalemindenom="15000" filter="&quot;Category&quot; ~ 'Admin|Education|Public|Post(?!Box)'"/>
        <rule label="Education" symbol="4" scalemaxdenom="15000" key="{b605fde3-cffd-4393-9137-a6cb23a9df1a}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Education'"/>
        <rule label="Government" symbol="5" scalemaxdenom="15000" key="{bae48e8a-e4b4-4825-b2ad-ab582bfc31d0}" scalemindenom="1" filter="&quot;Category&quot; = 'Admin'"/>
        <rule label="Police" symbol="6" scalemaxdenom="15000" key="{4117077e-1b76-4097-b140-54ee0150e7a5}" scalemindenom="1" filter="&quot;Category&quot; = 'Police'"/>
        <rule label="Post" symbol="7" scalemaxdenom="15000" key="{d7625bd6-8863-453e-93bc-46d80939aeef}" scalemindenom="1" filter="&quot;Category&quot; = 'Post'"/>
      </rules>
      <symbols>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="0" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="231,139,139,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="1" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="231,139,139,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="1" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="emergency/amenity=fire_station.svg" name="name"/>
              <Option type="QString" value="-0.10000000000000001,-0.10000000000000001" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,255" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="2" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="231,139,139,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="cross_fill" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="solid" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="2.4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="3" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="175,160,129,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="1" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="4" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="175,160,129,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_library.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="5" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="175,160,129,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="tourist/tourist_museum.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="6" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="175,160,129,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_police.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
        <symbol is_animated="0" type="marker" clip_to_extent="1" name="7" frame_rate="10" alpha="1" force_rhr="0">
          <data_defined_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </data_defined_properties>
          <layer pass="1" enabled="1" locked="0" class="SimpleMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="square" name="cap_style"/>
              <Option type="QString" value="175,160,129,255" name="color"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="bevel" name="joinstyle"/>
              <Option type="QString" value="circle" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="no" name="outline_style"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="4" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
          <layer pass="0" enabled="1" locked="0" class="SvgMarker">
            <Option type="Map">
              <Option type="QString" value="0" name="angle"/>
              <Option type="QString" value="255,255,255,255" name="color"/>
              <Option type="QString" value="0" name="fixedAspectRatio"/>
              <Option type="QString" value="1" name="horizontal_anchor_point"/>
              <Option type="QString" value="amenity/amenity_post_box.svg" name="name"/>
              <Option type="QString" value="0,0" name="offset"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
              <Option type="QString" value="MM" name="offset_unit"/>
              <Option type="QString" value="35,35,35,0" name="outline_color"/>
              <Option type="QString" value="0" name="outline_width"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
              <Option type="QString" value="MM" name="outline_width_unit"/>
              <Option name="parameters"/>
              <Option type="QString" value="diameter" name="scale_method"/>
              <Option type="QString" value="3.6" name="size"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
              <Option type="QString" value="MM" name="size_unit"/>
              <Option type="QString" value="1" name="vertical_anchor_point"/>
            </Option>
            <data_defined_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </data_defined_properties>
          </layer>
        </symbol>
      </symbols>
    </renderer-v2>
    <symbol is_animated="0" type="marker" clip_to_extent="1" name="centerSymbol" frame_rate="10" alpha="1" force_rhr="0">
      <data_defined_properties>
        <Option type="Map">
          <Option type="QString" value="" name="name"/>
          <Option name="properties"/>
          <Option type="QString" value="collection" name="type"/>
        </Option>
      </data_defined_properties>
      <layer pass="0" enabled="0" locked="0" class="SimpleMarker">
        <Option type="Map">
          <Option type="QString" value="0" name="angle"/>
          <Option type="QString" value="square" name="cap_style"/>
          <Option type="QString" value="245,75,80,0" name="color"/>
          <Option type="QString" value="1" name="horizontal_anchor_point"/>
          <Option type="QString" value="bevel" name="joinstyle"/>
          <Option type="QString" value="circle" name="name"/>
          <Option type="QString" value="0,0" name="offset"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
          <Option type="QString" value="MM" name="offset_unit"/>
          <Option type="QString" value="35,35,35,0" name="outline_color"/>
          <Option type="QString" value="solid" name="outline_style"/>
          <Option type="QString" value="0" name="outline_width"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
          <Option type="QString" value="MM" name="outline_width_unit"/>
          <Option type="QString" value="diameter" name="scale_method"/>
          <Option type="QString" value="1" name="size"/>
          <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
          <Option type="QString" value="MM" name="size_unit"/>
          <Option type="QString" value="1" name="vertical_anchor_point"/>
        </Option>
        <data_defined_properties>
          <Option type="Map">
            <Option type="QString" value="" name="name"/>
            <Option name="properties"/>
            <Option type="QString" value="collection" name="type"/>
          </Option>
        </data_defined_properties>
      </layer>
    </symbol>
  </renderer-v2>
  <labeling type="rule-based">
    <rules key="{7fce86fa-27a1-49d9-a33b-2f8ad7963efa}">
      <rule scalemaxdenom="20000" description="Emergency" key="{ad578a92-4810-40d0-840d-9dd90cd54f32}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Fire|Health'">
        <settings calloutType="simple">
          <text-style fontFamily="Microsoft JhengHei UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="239,144,144,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Bold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Aa" textOpacity="1" fontWeight="75">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="0" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule scalemaxdenom="15000" description="Goverment" key="{3858e04f-74dd-4e52-be7a-30ef1cdd293b}" scalemindenom="1" filter="&quot;Category&quot; ~ 'Admin|Education|Police|Post(?!Box)'">
        <settings calloutType="simple">
          <text-style fontFamily="Microsoft JhengHei UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="162,137,88,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="9" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Bold" fieldName="if(regexp_match(&quot;Name&quot;, '^(?:\\p{Latin}|\\p{Z}|\\p{N}|\\p{P})+$'), &quot;Name&quot;, trim(regexp_replace(&quot;Name&quot;, concat('(.{1,', if(length(&quot;Name&quot;) > 6, to_int(length(&quot;Name&quot;) / 2), 6), '})'), '\\1\n')))" capitalization="0" legendString="Aa" textOpacity="1" fontWeight="75">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="1" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar="" decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="0" autoWrapLength="12" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="0" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="3" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="5" repeatDistanceUnits="MM" placement="6" priority="7" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="3" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option type="Map" name="properties">
                <Option type="Map" name="OffsetQuad">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="if(@id % 2 = 0, 5, 3)" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
                <Option type="Map" name="PredefinedPositionOrder">
                  <Option type="bool" value="true" name="active"/>
                  <Option type="QString" value="'R,L'" name="expression"/>
                  <Option type="int" value="3" name="type"/>
                </Option>
              </Option>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
            </Option>
          </callout>
        </settings>
      </rule>
      <rule key="{b46b6333-8b65-498c-89ce-cdf666303d8c}" filter="ELSE">
        <settings calloutType="simple">
          <text-style fontFamily="Microsoft JhengHei UI" fontSizeMapUnitScale="3x:0,0,0,0,0,0" isExpression="1" textColor="255,255,255,255" fontKerning="1" multilineHeightUnit="Percentage" forcedBold="0" fontStrikeout="0" fontItalic="0" fontLetterSpacing="0" fontUnderline="0" textOrientation="horizontal" forcedItalic="0" fontSizeUnit="Point" previewBkgrdColor="255,255,255,255" fontWordSpacing="0" blendMode="0" fontSize="1" useSubstitutions="0" multilineHeight="1" allowHtml="0" namedStyle="Regular" fieldName="0" capitalization="0" legendString="Aa" textOpacity="0" fontWeight="50">
            <families/>
            <text-buffer bufferColor="250,250,250,255" bufferBlendMode="0" bufferSizeUnits="MM" bufferDraw="0" bufferSize="0.5" bufferNoFill="1" bufferOpacity="0.69999999999999996" bufferSizeMapUnitScale="3x:0,0,0,0,0,0" bufferJoinStyle="128"/>
            <text-mask maskSizeUnits="MM" maskEnabled="0" maskSizeMapUnitScale="3x:0,0,0,0,0,0" maskType="0" maskedSymbolLayers="" maskSize="0" maskOpacity="1" maskJoinStyle="128"/>
            <background shapeSVGFile="" shapeBorderWidthUnit="Point" shapeSizeType="0" shapeOffsetUnit="Point" shapeOffsetY="0" shapeOpacity="1" shapeOffsetX="0" shapeDraw="0" shapeRadiiX="0" shapeType="0" shapeRadiiMapUnitScale="3x:0,0,0,0,0,0" shapeBorderWidthMapUnitScale="3x:0,0,0,0,0,0" shapeRotation="0" shapeJoinStyle="64" shapeRadiiY="0" shapeBorderWidth="0" shapeBlendMode="0" shapeSizeMapUnitScale="3x:0,0,0,0,0,0" shapeRadiiUnit="Point" shapeBorderColor="128,128,128,255" shapeOffsetMapUnitScale="3x:0,0,0,0,0,0" shapeRotationType="0" shapeFillColor="255,255,255,255" shapeSizeUnit="Point" shapeSizeX="0" shapeSizeY="0">
              <symbol is_animated="0" type="marker" clip_to_extent="1" name="markerSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleMarker">
                  <Option type="Map">
                    <Option type="QString" value="0" name="angle"/>
                    <Option type="QString" value="square" name="cap_style"/>
                    <Option type="QString" value="152,125,183,255" name="color"/>
                    <Option type="QString" value="1" name="horizontal_anchor_point"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="circle" name="name"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="35,35,35,255" name="outline_color"/>
                    <Option type="QString" value="solid" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="outline_width_map_unit_scale"/>
                    <Option type="QString" value="MM" name="outline_width_unit"/>
                    <Option type="QString" value="diameter" name="scale_method"/>
                    <Option type="QString" value="2" name="size"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="size_map_unit_scale"/>
                    <Option type="QString" value="MM" name="size_unit"/>
                    <Option type="QString" value="1" name="vertical_anchor_point"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
              <symbol is_animated="0" type="fill" clip_to_extent="1" name="fillSymbol" frame_rate="10" alpha="1" force_rhr="0">
                <data_defined_properties>
                  <Option type="Map">
                    <Option type="QString" value="" name="name"/>
                    <Option name="properties"/>
                    <Option type="QString" value="collection" name="type"/>
                  </Option>
                </data_defined_properties>
                <layer pass="0" enabled="1" locked="0" class="SimpleFill">
                  <Option type="Map">
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="border_width_map_unit_scale"/>
                    <Option type="QString" value="255,255,255,255" name="color"/>
                    <Option type="QString" value="bevel" name="joinstyle"/>
                    <Option type="QString" value="0,0" name="offset"/>
                    <Option type="QString" value="3x:0,0,0,0,0,0" name="offset_map_unit_scale"/>
                    <Option type="QString" value="MM" name="offset_unit"/>
                    <Option type="QString" value="128,128,128,255" name="outline_color"/>
                    <Option type="QString" value="no" name="outline_style"/>
                    <Option type="QString" value="0" name="outline_width"/>
                    <Option type="QString" value="Point" name="outline_width_unit"/>
                    <Option type="QString" value="solid" name="style"/>
                  </Option>
                  <data_defined_properties>
                    <Option type="Map">
                      <Option type="QString" value="" name="name"/>
                      <Option name="properties"/>
                      <Option type="QString" value="collection" name="type"/>
                    </Option>
                  </data_defined_properties>
                </layer>
              </symbol>
            </background>
            <shadow shadowRadiusMapUnitScale="3x:0,0,0,0,0,0" shadowRadiusUnit="MM" shadowRadius="1.5" shadowOffsetDist="1" shadowUnder="0" shadowOffsetAngle="135" shadowRadiusAlphaOnly="0" shadowOpacity="0.69999999999999996" shadowBlendMode="6" shadowDraw="0" shadowColor="0,0,0,255" shadowOffsetUnit="MM" shadowOffsetMapUnitScale="3x:0,0,0,0,0,0" shadowOffsetGlobal="1" shadowScale="100"/>
            <dd_properties>
              <Option type="Map">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
            </dd_properties>
            <substitutions/>
          </text-style>
          <text-format rightDirectionSymbol=">" addDirectionSymbol="0" placeDirectionSymbol="0" wrapChar=" " decimals="3" multilineAlign="3" useMaxLineLengthForAutoWrap="1" autoWrapLength="0" plussign="0" leftDirectionSymbol="&lt;" reverseDirectionSymbol="0" formatNumbers="0"/>
          <placement lineAnchorTextPoint="FollowPlacement" offsetType="1" repeatDistance="0" preserveRotation="1" labelOffsetMapUnitScale="3x:0,0,0,0,0,0" xOffset="0" centroidInside="0" geometryGeneratorEnabled="0" maxCurvedCharAngleIn="25" geometryGeneratorType="PointGeometry" overlapHandling="PreventOverlap" yOffset="0" quadOffset="4" repeatDistanceUnits="MM" placement="1" priority="5" offsetUnits="MM" centroidWhole="0" overrunDistanceMapUnitScale="3x:0,0,0,0,0,0" dist="0" geometryGenerator="" distUnits="MM" predefinedPositionOrder="TR,TL,BR,BL,R,L,TSR,BSR" lineAnchorType="0" layerType="PointGeometry" polygonPlacementFlags="2" rotationAngle="0" lineAnchorClipping="0" overrunDistance="0" distMapUnitScale="3x:0,0,0,0,0,0" repeatDistanceMapUnitScale="3x:0,0,0,0,0,0" placementFlags="10" fitInPolygonOnly="0" maxCurvedCharAngleOut="-25" allowDegraded="0" overrunDistanceUnit="MM" lineAnchorPercent="0.5" rotationUnit="AngleDegrees"/>
          <rendering fontLimitPixelSize="0" fontMaxPixelSize="10000" scaleMax="0" limitNumLabels="0" scaleVisibility="0" minFeatureSize="0" mergeLines="0" maxNumLabels="2000" unplacedVisibility="0" labelPerPart="0" obstacleType="1" upsidedownLabels="0" drawLabels="1" scaleMin="0" obstacleFactor="1.6000000000000001" zIndex="0" obstacle="1" fontMinPixelSize="3"/>
          <dd_properties>
            <Option type="Map">
              <Option type="QString" value="" name="name"/>
              <Option name="properties"/>
              <Option type="QString" value="collection" name="type"/>
            </Option>
          </dd_properties>
          <callout type="simple">
            <Option type="Map">
              <Option type="QString" value="pole_of_inaccessibility" name="anchorPoint"/>
              <Option type="int" value="0" name="blendMode"/>
              <Option type="Map" name="ddProperties">
                <Option type="QString" value="" name="name"/>
                <Option name="properties"/>
                <Option type="QString" value="collection" name="type"/>
              </Option>
              <Option type="bool" value="false" name="drawToAllParts"/>
              <Option type="QString" value="0" name="enabled"/>
              <Option type="QString" value="point_on_exterior" name="labelAnchorPoint"/>
              <Option type="QString" value="&lt;symbol is_animated=&quot;0&quot; type=&quot;line&quot; clip_to_extent=&quot;1&quot; name=&quot;symbol&quot; frame_rate=&quot;10&quot; alpha=&quot;1&quot; force_rhr=&quot;0&quot;>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;layer pass=&quot;0&quot; enabled=&quot;1&quot; locked=&quot;0&quot; class=&quot;SimpleLine&quot;>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;align_dash_pattern&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;square&quot; name=&quot;capstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;5;2&quot; name=&quot;customdash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;customdash_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;customdash_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;dash_pattern_offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;dash_pattern_offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;dash_pattern_offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;draw_inside_polygon&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;bevel&quot; name=&quot;joinstyle&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;60,60,60,255&quot; name=&quot;line_color&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;solid&quot; name=&quot;line_style&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0.3&quot; name=&quot;line_width&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;line_width_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;offset&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;offset_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;offset_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;ring_filter&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_end&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_end_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_end_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;trim_distance_start&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;trim_distance_start_map_unit_scale&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;MM&quot; name=&quot;trim_distance_start_unit&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;tweak_dash_pattern_on_corners&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;0&quot; name=&quot;use_custom_dash&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;3x:0,0,0,0,0,0&quot; name=&quot;width_map_unit_scale&quot;/>&lt;/Option>&lt;data_defined_properties>&lt;Option type=&quot;Map&quot;>&lt;Option type=&quot;QString&quot; value=&quot;&quot; name=&quot;name&quot;/>&lt;Option name=&quot;properties&quot;/>&lt;Option type=&quot;QString&quot; value=&quot;collection&quot; name=&quot;type&quot;/>&lt;/Option>&lt;/data_defined_properties>&lt;/layer>&lt;/symbol>" name="lineSymbol"/>
              <Option type="double" value="0" name="minLength"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="minLengthMapUnitScale"/>
              <Option type="QString" value="MM" name="minLengthUnit"/>
              <Option type="double" value="0" name="offsetFromAnchor"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromAnchorMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromAnchorUnit"/>
              <Option type="double" value="0" name="offsetFromLabel"/>
              <Option type="QString" value="3x:0,0,0,0,0,0" name="offsetFromLabelMapUnitScale"/>
              <Option type="QString" value="MM" name="offsetFromLabelUnit"/>
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
