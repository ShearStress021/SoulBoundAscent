# Soulbound Ascent — Full Semester Plan

**Team:** 3 Members  
**Engine:** Unity (PC)  
**Duration:** 16 Weeks (4 Months)  
**Role assignments:**

| Member | Primary Domain | Cross-cutting |
|--------|---------------|---------------|
| **M1** | Combat systems, unit AI, battle logic | Data architecture, save/load |
| **M2** | UI, town menus, roster progression | UX flow, event wiring |
| **M3** | Content, assets, balancing, presentation | Build pipeline, documentation |

---

## Sprint 1: Combat Prototype (Weeks 1–2)

> **Exit goal:** Player can deploy 4 units on a 5×6 grid, start auto battle, watch units move and auto-attack, pause to inspect a unit's stats, and resolve a win or loss on one test floor without crashes or console errors.

All three members work on the same battle scene from different angles. No town, no roster — hardcoded test data.

---

### Week 1 — Foundation (Days 1–5)

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 1.1 | Unity project scaffold, scene, camera, grid renderer | M | M1 | — | Empty 5×6 grid visible in Game view, camera framed top-down, grid cells are clickable in editor |
| 1.2 | Unit data model + state machine (Idle/Moving/Attacking/Dead) | M | M1 | 1.1 | Unit prefab has HP, position, state enum; state transitions fire correctly on test calls |
| 1.3 | Deployment UI — click grid slot to place hero | M | M2 | 1.1 | Player clicks empty slot → hero icon appears; clicking occupied slot swaps or deselects |
| 1.4 | Placeholder unit prefabs — 5 job colors + 3 enemy shapes | M | M3 | 1.1 | Distinct visual per job (colored cubes/shapes); enemies are different color/shape set |
| 1.5 | Basic unit movement — move toward nearest enemy cell-by-cell | M | M1 | 1.2 | Units traverse grid toward nearest enemy; stops when adjacent; no teleporting |
| 1.6 | HUD — HP bars + team status panel | M | M2 | 1.2 | HP bar floats above each unit; panel on screen edge shows alive/dead count |
| 1.7 | Floor 1 enemy wave ScriptableObject | S | M3 | 1.2 | SO defines enemy count, positions, job slots, stats; loadable by battle scene |
| 1.8 | Enemy base AI — move toward nearest hero and stop in range | M | M3 | 1.5 | Enemies move toward heroes using same movement system |

---

### Week 2 — Combat Resolution (Days 6–10)

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 1.9 | Auto-targeting system — find nearest valid enemy, switch on death | M | M1 | 1.5 | Unit picks closest target; retargets when current target dies |
| 1.10 | Damage system + formula (Atk vs Def/Armor) | M | M1 | 1.9 | Units deal damage per formula; floating damage numbers appear on hit |
| 1.11 | Win/loss condition — all enemies dead OR all heroes dead | S | M1 | 1.10 | Battle ends, state transitions to Victory or Defeat; no orphaned units |
| 1.12 | Type advantage triangle (Warrior>Archer>Mage>Guardian>Warrior, Healer neutral) | S | M1 | 1.10 | Correct damage modifier (+20%/-10%) applied per matchup in combat log |
| 1.13 | Win/loss result screen with stats | M | M2 | 1.11 | Modal shows outcome, kill counts, damage dealt, "Continue" button |
| 1.14 | Pause button + unit inspection panel | M | M2 | 1.3 | Pause freezes battle; clicking unit opens panel showing name, HP, stats, current action |
| 1.15 | Combat log — scrollable action feed | M | M2 | 1.10 | Text feed shows "Warrior attacks Goblin for 12 damage" — scrolls with newest at bottom |
| 1.16 | Full Floor 1 integration test + stat tuning | M | M3 | 1.12, 1.11 | Deploy → battle → win/loss complete without errors; floor feels winnable with 4 heroes |
| 1.17 | Simple hit VFX (flash/particle on damage) | S | M3 | 1.10 | Brief white flash or particle burst on each hit; not distracting |

---

### Sprint 1 Risk Register

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Grid + movement takes longer than expected | Medium | High | Clamp to cell-by-cell movement, no pathfinding A* — simple Lerp/teleport between cells |
| Damage formula feels bad (no visible effect) | Medium | Medium | Add floating damage numbers early (Day 7) to get immediate feedback |
| Pause/inspection is low priority but complex | Medium | Low | Ship basic stat readout first; rich inspection is stretch |
| Unity project setup issues (version control, packages) | High | Medium | M1 sets up Git LFS + Unity .gitignore on Day 1; test build before end of Day 1 |
| 3 members on same scene → merge conflicts | High | Medium | M1 owns scene prefab; M2/M3 work in prefab variants or separate scenes; merge daily standup |

---

### Critical Path (Sprint 1)

```
1.1 (Grid) → 1.2 (Unit Model) → 1.5 (Movement) → 1.9 (Targeting) → 1.10 (Damage) → 1.11 (Win/Loss)
                                                                                         ↓
                                                                                   1.16 (Integration)
```

**Parallel tracks:**
- M2: 1.3 → 1.6 → 1.13 → 1.14 → 1.15 (independent after 1.1)
- M3: 1.4 → 1.7 → 1.8 → (joins 1.16) + 1.17

---

## Sprint 2: Combat Hardening (Weeks 3–4)

> **Exit goal:** Two test floors are playable, type advantage and basic synergies work, combat is readable through pause inspection + combat log, and the scene structure supports swapping in different floor configs.

After proving the core loop, this sprint hardens the battle system for town integration.

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 2.1 | Synergy system — Job + Attribute, 2-unit thresholds | M | M1 | 1.12 | 2+ same job → synergy activates; buff applies to all matching units; UI indicator exists |
| 2.2 | Floor config loader — swap floors without scene change | M | M1 | 1.7 | Battle scene reads any FloorConfig SO; floors loadable by index |
| 2.3 | Floor 2 enemy config + placement | S | M3 | 2.2 | Floor 2 has different enemy composition; harder than Floor 1 |
| 2.4 | Battle speed controls (1x / 2x / 4x) | S | M2 | 1.10 | Buttons in HUD change Time.timeScale; visible speed indicator |
| 2.5 | Animancer/Animation placeholders — attack swing, death fade | M | M3 | 1.4 | Units play simple animation on attack (scale punch) and death (fade out) |
| 2.6 | Grid highlight — valid deployment zones + enemy zones | S | M2 | 1.3 | Blue tint for player zone, red for enemy zone; occupied cells are distinct |
| 2.7 | Restart battle button | S | M2 | 1.13 | "Retry" reloads current floor; no scene reload needed |
| 2.8 | Unit death animation + removal from grid | S | M3 | 2.5 | Unit fades/falls, grid cell frees up visually |
| 2.9 | Battle scene refactor for reuse (clean separation of battle init, run, teardown) | M | M1 | 2.2 | BattleManager can be called from outside scene; no hardcoded references |
| 2.10 | Input guard — prevent actions during auto-battle (only pause allowed) | S | M2 | 1.14 | Clicking grid during battle does nothing; pause is only interaction |
| 2.11 | Screen shake on heavy hits (placeholder juice) | S | M3 | 1.17 | Camera shakes briefly on critical hits or hero death |
| 2.12 | End-of-Sprint integration + bug bash (half day) | M | ALL | 2.1–2.11 | All three run full playthrough; bugs filed; exit goal verified |

---

## Month 2: Hero + Town Systems (Weeks 5–8)

> **Exit goal:** Player can summon raw heroes, train them into jobs, assign squads, view the roster, and prepare for floors through town menus — then take that roster into battle and see job/squad stats reflected in combat.

### Role split after Sprint 2

| Member | Focus |
|--------|-------|
| **M1** | Hero lifecycle (summon → train → squad), combat integration (roster feeds battle), save/load |
| **M2** | Town hub UI, building menus, roster display, navigation flow |
| **M3** | Town building configs, enemy variants, floor 3 draft, placeholder building art |

---

### Week 5 — Hero Data Model & Summoning

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 3.1 | Hero data model — StarRating, InnateAttribute, Job, Squad, stats, level, XP | L | M1 | 2.9 | Hero SO/class holds all MVP fields; serializable for save |
| 3.2 | Summoning Gate logic — generate raw hero with star + attribute | M | M1 | 3.1 | Summon button creates hero with random star (1-5 weighted) and attribute; hero is untrained |
| 3.3 | Town Hub scene layout — building buttons, navigation structure | M | M2 | 2.9 | Clickable building buttons; each transitions to its menu; back button returns to hub |
| 3.4 | Summoning Gate UI — summon button, result display, hero info card | M | M2 | 3.2 | Shows summoned hero's star, attribute, "Untrained" state; confirm adds to roster |
| 3.5 | Placeholder building icons + town background | M | M3 | — | Colored rectangle buildings with labels; simple town backdrop |
| 3.6 | Roster data structure — list of owned heroes, alive/dead/reviving states | M | M1 | 3.1 | RosterManager holds List<Hero>; add/remove/query by state works |

### Week 6 — Training & Squad System

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 3.7 | Training Facility logic — choose job, apply stat changes | M | M1 | 3.1 | Untrained hero → pick job → gains job base stats; job-locked thereafter |
| 3.8 | Level-up system — XP gain after battle, stat growth per job | M | M1 | 3.7 | Victory grants XP; level up triggers stat increase based on job growth curve |
| 3.9 | Training Facility UI — job selection grid, stat preview, confirm/cancel | M | M2 | 3.7 | Shows 5 job cards with stat preview on hover; confirm locks in job |
| 3.10 | Squad data — 2 squads per job, stats, passive skill | L | M1 | 3.7 | Each job has 2 SquadSO; squad grants flat stats + one passive; level follows hero level |
| 3.11 | Squad selection UI — choose squad before deployment | M | M2 | 3.10 | Before battle, hero card has squad dropdown showing compatible squads |
| 3.12 | Floor 3 enemy config + new enemy variant | M | M3 | 2.3 | Floor 3 introduces one new enemy type; different formation layout |

### Week 7 — Battle Integration & Roster Display

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 3.13 | Roster → Battle feed — selected heroes appear in deployment UI | M | M1 | 3.6, 2.9 | Heroes chosen in town appear in pre-battle deployment panel |
| 3.14 | Job stats apply in combat — damage, HP, speed vary by job | M | M1 | 3.7, 1.10 | Warrior deals more phys dmg, Mage deals magic dmg; values match hero's stats |
| 3.15 | Squad passives activate in battle | M | M1 | 3.10 | Squad's passive skill triggers at correct time (e.g., Clerics: +heal) |
| 3.16 | Roster list UI — scrollable grid of owned heroes | M | M2 | 3.6 | Shows all heroes with star, job icon, level; click for detail |
| 3.17 | Hero detail panel — full stats, equipment slot, squad | M | M2 | 3.1 | In roster, clicking a hero shows full stat breakdown |
| 3.18 | Save/load system — serialize roster, town state, floor progress | L | M1 | 3.6 | Binary/JSON save file; load restores exact state; save on exit + manual save button |

### Week 8 — Integration & Buffer

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 3.19 | Full loop integration: Town → Roster → Deploy → Battle → Results → Town | XL | ALL | 3.13–3.18 | Complete player flow without scene reload hacks; data persists correctly through loop |
| 3.20 | Bug fixing + edge cases (empty roster, all heroes dead) | M | ALL | 3.19 | UI handles empty states; game doesn't break on edge cases |
| 3.21 | Floor 1–3 balance pass with real hero stats | M | M3 | 3.14, 2.3 | Floors are winnable with 4 trained heroes; tight but fair |
| 3.22 | Town background art pass (tiled buildings, nicer layout) | S | M3 | 3.5 | Buildings have distinct silhouettes; town feels like a place |

---

### Month 2 Risk Register

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Save/load is too complex for M1 alone | Medium | High | Use simple JSON serialization first; binary if time permits; defer cloud/encryption |
| Town UI scope grows (too many screens) | High | Medium | Lock to 4 MVP buildings (Summon, Train, Blacksmith — skip Shrine/Apothecary until Month 3) |
| Roster → Battle data pipe breaks | Medium | High | Contract-first: agree on RosterManager API before either side codes |
| Synergies not yet in battle | Low | Medium | Synergy system was Sprint 2; verify it's wired before Week 8 integration |
| 3.19 (full loop) is XL — risky in one week | High | High | Start integration mid-Week 7; reserve Week 8 entirely for it |

---

## Month 3: Content + Consequences (Weeks 9–12)

> **Exit goal:** Player can complete a full 5-floor tower run with hero deaths, shrine revival, equipment locking/recovery, escalating Tower Malice, and at least one invasion defense — experiencing the full consequence loop.

### Role split

| Member | Focus |
|--------|-------|
| **M1** | Death/revival/inheritance systems, equipment logic, Tower Malice, invasion/protection mode |
| **M2** | Shrine UI, equipment UI, invasion warning UI, item crafting UI, restaurant menu |
| **M3** | Floors 4–5 content, invasion encounter configs, restaurant/item data, audio placeholders |

---

### Week 9 — Death, Revival, Equipment

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 4.1 | Soul system — hero death creates revivable soul with expiry timer | M | M1 | 3.6 | Dead hero moves to "Soul" state; timer counts down; expiry = permanent death |
| 4.2 | Shrine revival logic — cost, time-to-revive based on hero power | M | M1 | 4.1 | Reviving consumes resource; stronger heroes take longer; soul expires if timer hits 0 |
| 4.3 | Soul inheritance — permanent loss → XP to target hero | M | M1 | 4.1 | Choosing inheritance deletes dead hero, grants XP to chosen living hero |
| 4.4 | Equipment system — personal fixed equipment, enhance, repair | L | M1 | 3.1 | Hero gets weapon/armor on job pick; enhance raises stats; repair fixes durability |
| 4.5 | Equipment locking — on party wipe, equipment trapped on floor | M | M1 | 4.4 | Full wipe → equipment state = Locked; clearing floor → Recovered |
| 4.6 | Shrine UI — revive confirm, inheritance target picker, soul list | M | M2 | 4.2, 4.3 | Shows souls with timer bars; revive button with cost; inheritance target selector |
| 4.7 | Equipment UI — hero equip panel, enhance/repair buttons, locked indicator | M | M2 | 4.4 | Hero detail shows equipment; lock state visible; enhance shows cost/preview |
| 4.8 | Blacksmith building screen — equipment enhancement | M | M2 | 4.7 | Select hero → see equipment → enhance button with resource cost |

### Week 10 — Tower Malice & Invasion

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 4.9 | Tower Malice system — permanent hero loss increases Malice level | M | M1 | 4.1 | Malice level tracks permanent deaths; each level adds difficulty pressure |
| 4.10 | Malice effects — invasion speeds up, enemies gain stat bonus | M | M1 | 4.9 | Low Malice = slow invasion; Med = +enemy stats; High = extra enemy on boss floor |
| 4.11 | Invasion system — timer/floor count triggers invasion event | L | M1 | 4.10 | If player clears < X floors in Y days, upper floor invades a cleared floor |
| 4.12 | Protection mode — defend invaded floor with current roster | M | M1 | 4.11, 1.11 | Same battle scene but labeled "Protection"; win = reclaim floor, lose = floor occupied |
| 4.13 | Invasion warning UI — popup before protection battle | S | M2 | 4.11 | "Floor 3 is under invasion!" modal; shows enemy preview; fight or retreat options |
| 4.14 | Malice display in town HUD | S | M2 | 4.9 | Permanent icon showing current Malice level + tooltip |
| 4.15 | Invasion encounter configs (3 variants) | M | M3 | 4.11 | SOs for invasion waves; weaker than full floor but scale with Malice |

### Week 11 — Items, Restaurant, Floors 4–5

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 4.16 | Consumable item system — pre-battle equip, auto-trigger during battle | M | M1 | 1.10 | Hero equips 1 consumable; item auto-fires on trigger condition (HP < 70%, etc.) |
| 4.17 | 3 MVP items — Elixir, Cleansing Herb, Barrier Charm | M | M1 | 4.16 | Each item triggers correctly in combat; effect applies, item consumed |
| 4.18 | Apothecary UI — view/equip consumables | M | M2 | 4.17 | Shows available items; equip to hero slot; limited to 1 per hero |
| 4.19 | Restaurant meal system — pre-battle buff, 1 meal per run | M | M1 | 4.16 | Meal selected before battle; buff applies to all/front/backline; expires after attempt |
| 4.20 | Restaurant UI — meal selection with effect preview | S | M2 | 4.19 | 4 meal options showing buff text; confirm locks in; only one active |
| 4.21 | Floor 4 enemy config + boss enemy type | M | M3 | 3.12 | New enemy prefab; floor has mini-boss with more HP + unique passive |
| 4.22 | Floor 5 — final boss encounter config | M | M3 | 4.21 | Boss has two phases or unique mechanic; hardest floor |
| 4.23 | 4 meal SOs + 3 item SOs | S | M3 | 4.16, 4.19 | Data assets for items and meals; balanced values |

### Week 12 — Full Integration & Content Pass

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 4.24 | Full tower run integration test — Floor 1 → 5 with death/revival/invasion | XL | ALL | 4.1–4.23 | Complete playthrough from fresh save to Floor 5 clear; all systems interact correctly |
| 4.25 | Floor difficulty tuning — all 5 floors with real roster | L | M3 | 4.21–4.23 | Floors 1-5 progressive difficulty; final boss is challenging but beatable |
| 4.26 | Edge case pass — empty roster, all dead, no resources | M | ALL | 4.24 | All failure states have graceful handling; player can always recover |
| 4.27 | Placeholder audio — combat SFX, UI clicks, town ambience | M | M3 | — | Hit sounds, death sounds, button clicks, ambient loop from free sources |
| 4.28 | Tutorial prompt system — first-time tooltips | M | M2 | 4.24 | On first summon, tooltip shows; on first death, tooltip shows; skippable |

---

### Month 3 Risk Register

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Invasion system is too complex for MVP | High | High | Cut to "1 occupied floor at a time, simple timer" — defer multi-floor invasions |
| 5 floors of content overwhelms M3 | Medium | High | Floors 1–3 reuse existing enemies; only floors 4–5 need new prefabs/boss |
| Equipment + Malice + Invasion = too many systems in one month | High | High | Prioritize: revival > equipment > items > Malice > invasion (cut invasion if behind) |
| Death spiral — player loses all heroes and can't continue | Medium | High | Shrine emergency recovery + one free resurrection per campaign as safety net |
| Audio placeholder search takes too long | Low | Low | Use free sound effect packs (freesound.org, Soniss) — no custom audio |

---

## Month 4: Polish & Presentation (Weeks 13–16)

> **Exit goal:** A new player can launch the game, understand what to do through UI clarity and optional tooltips, play through floors 1–5 with readable combat feedback, and complete the demo without developer explanation.

### All hands on deck — no new systems

| # | Task | Effort | Owner | Depends On | Success Criteria |
|---|------|--------|-------|------------|-----------------|
| 5.1 | UI style pass — consistent fonts, colors, spacing, button states | M | M2 | 4.24 | All screens use same palette/font; buttons have hover/press states; no placeholder grayboxes |
| 5.2 | Synergy panel in battle prep — show active synergies | M | M2 | 2.1 | Before battle, panel shows active Job + Attribute synergies; tooltip with bonus values |
| 5.3 | Synergy panel in roster — show what would activate | M | M2 | 5.2 | In squad selection, panel previews synergies for current deployment |
| 5.4 | Battle VFX upgrade — better hit effects, death particles, screen shake tuning | M | M3 | 2.11 | Hits feel impactful; deaths have clear visual; screen shake is configurable |
| 5.5 | Combat log readability pass — color-coded entries, icons, collapse by type | M | M2 | 1.15 | Damage in red, heals in green, buffs in blue; collapse same-type entries |
| 5.6 | Audio integration — wire all SFX to events, add volume slider | M | M3 | 4.27 | Every UI action + combat event has sound; AudioManager with volume control |
| 5.7 | Music — town theme + battle theme (free/CC licensed) | S | M3 | 5.6 | Background music plays in town and battle; smooth crossfade on transition |
| 5.8 | Balance pass — all 5 floors, all jobs, all items, all meals | L | M3 | 4.25 | No job is strictly inferior; items are worth using; floors are clearable without grind |
| 5.9 | Bug fixing sprint — systematic QA pass | L | ALL | 5.1–5.8 | All critical bugs fixed; no crashes; no softlocks; edges cases handled |
| 5.10 | Save/load robustness — test save corruption, multiple saves, load edge cases | M | M1 | 3.18 | Save/load works across restarts; no data loss on crash (save on checkpoint) |
| 5.11 | Loading screen + transition effects between scenes | S | M2 | 5.1 | Smooth loading between Town → Battle → Town; progress bar or spinner |
| 5.12 | Build pipeline — Windows standalone build, single-click build script | M | M3 | — | One command produces standalone .exe; all assets included; no missing references |
| 5.13 | Documentation — gameplay guide, build instructions, asset attribution log | M | M3 | 5.12 | README with controls + systems; attribution file for all assets/audio |
| 5.14 | Final playtest — 3 external players, observe, collect feedback | M | ALL | 5.12 | Each team member watches a new player; 3 bugs/confusions fixed from each session |
| 5.15 | Presentation build — locked branch, release build, recorded demo video | M | ALL | 5.14 | Standalone build ready for professor; 3-min gameplay video; slide deck updated |

---

### Month 4 Risk Register

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Bug fixing takes all 4 weeks, no polish | High | High | Week 13-14 polish, Week 15 bugs, Week 16 build — swap priority if needed |
| External playtest reveals fundamental confusion | Medium | High | Do internal playtest first (Week 14) to catch obvious issues before external |
| Build breaks on different machines | Medium | High | Test build on 2 other machines by Week 15 |
| Audio licensing unclear | Low | Medium | Use only CC0/public domain sources; log every source in attribution file |
| Presentation week crunch | Medium | Medium | Lock build by Monday of Week 16; only critical bugfixes after that |

---

## Full Semester Dependency Map

```
Sprint 1 ──→ Sprint 2 ──→ Month 2 ──→ Month 3 ──→ Month 4
(Combat       (Hardened    (Hero+Town    (Content+     (Polish+
 Prototype)     Combat)      Systems)      Consequences) Presentation)
```

**Overarching critical path:**
1.1 → 1.2 → 1.5 → 1.9 → 1.10 → 1.11 → 2.2 → 3.1 → 3.6 → 3.7 → 3.10 → 3.13 → 3.19 → 4.1 → 4.4 → 4.9 → 4.11 → 4.24 → 5.8 → 5.14 → 5.15

This is the chain that must not slip. If this path is at risk, cut features on parallel branches, not nodes on this chain.

---

## Scope Safety Valve

If the team falls behind, cut in this order (never cut critical path nodes):

1. ❌ **Cut first:** Restaurant system (meals are nice-to-have)
2. ❌ **Cut next:** Multiple item types (ship only Elixir)
3. ❌ **Cut next:** Invasion mode (ship Subjugation only)
4. ❌ **Cut next:** Synergy display polish (ship basic synergy logic without fancy UI)
5. ❌ **Cut next:** Floor 5 boss complexity (make it a strong enemy, not two-phase)
6. ❌ **Cut next:** Audio/Music (silent but playable)
7. ❌ **Last resort:** Cut inheritance (ship revival only, no soul transfer)

---

## Summary of Effort by Milestone

| Milestone | M1 Days | M2 Days | M3 Days | Total Person-Days |
|-----------|---------|---------|---------|-------------------|
| Sprint 1 (Weeks 1-2) | 10 | 10 | 10 | 30 |
| Sprint 2 (Weeks 3-4) | 8 | 8 | 8 | 24 |
| Month 2 (Weeks 5-8) | 18 | 16 | 14 | 48 |
| Month 3 (Weeks 9-12) | 20 | 16 | 18 | 54 |
| Month 4 (Weeks 13-16) | 6 | 14 | 16 | 36 |
| **Total** | **62** | **64** | **66** | **192** |

Each member ≈ 60-66 working days out of 80 available. Buffer accounts for the remaining days (meetings, standups, demo prep, sick days).
