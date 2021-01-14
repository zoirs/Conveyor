using System;
using System.Collections.Generic;
using Line;

[Serializable]
public class LevelDto {
    public List<CompanyDto> companies = new List<CompanyDto>();
    public List<LineDto> lines = new List<LineDto>();
}