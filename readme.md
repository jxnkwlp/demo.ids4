
## 要点 

1，Claims 中 sub, name 是必须的  
2，User.Identity.Name 取值于 ClaimsIdentity 的 nameType， 系统默认是 ClaimTypes.Name
3, 标准 Scopes 中的 Profile 只包含 name, family_name, given_name, middle_name, nickname, preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, updated_at 。 如果要新增自定义的字段，需要 预先在 IdentityResource 中定义
4，