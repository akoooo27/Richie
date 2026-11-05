# Identity edges: Phase 4 test log

What was checked, what failed, what was fixed, and the retest. The canvas under test is **Richie Identity Edges** (https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge). Phase 4 started with 73 boards (assembly version `1793888623-5e41`). After the fix round there are **80 boards** (version `1793896016-b1a5`). Each board has a local mirror in `.design/edges/project/`.

## 1. What was checked

| Test | Owner (model) | Method | Report |
|---|---|---|---|
| Rig sweep and layout shift | WP4.1 (Sonnet) | QA rig (`.design/qa-rig`) on 73 boards × 10 viewports (320–1920 wide, 844×390 landscape, 200 % zoom) in light and dark. Three runs: a forced-dark build (`BOARD_THEME=dark`, every state under the dark tokens), forced colours in light and dark, and reduced motion. axe-core 4.10.2. Also drift and icon checks, a WCAG 1.4.12 text-spacing pass, and a layout-shift script that tracks the mascot, `h1` and primary button across every flow at 390 and 1440 | [`p4-rig.md`](p4-rig.md) |
| Keyboard and screen reader | WP4.2 (Opus) | Per board: accessibility-tree snapshot, Tab order with names and focus-style change, headings, landmarks, live regions, ARIA states, `lang`, titles. Checked against WCAG 2.2 AA (24 criteria listed in the report) and against each page’s spec focus and announcement rules | [`p4-a11y.md`](p4-a11y.md) |
| Contrast | WP4.3 (Haiku) | Every visible text node, and every control boundary, badge and focus indicator, measured against its effective background in the light and forced-dark builds at 390 and 1440. Cross-checked against `.design/edges/wp31/contrast-report.md` | [`p4-contrast.md`](p4-contrast.md) |
| Copy | WP4.4 (Sonnet) | Every board’s visible and screen-reader text, every spec copy table, copy-only state and email. Checked against the copy rules, the one-name table, consistency across the four areas, and drift between each board and its spec | [`p4-copy.md`](p4-copy.md) |
| State coverage and buildability | WP4.5 (Sonnet) | A state matrix per page (default, busy, each error, empty or success, mobile, dark). A buildability checklist per page (route, trigger, copy, actions, focus, live regions, limits, reveals, emails, mapping). 28 journeys walked for dead ends, plus a sweep for contradictions | [`p4-coverage.md`](p4-coverage.md) |

**Not testable here** (static mocks): real screen readers and speech input, the OS passkey sheets, the Google and Microsoft pages, clipboard and one-time-code autofill, the offline service worker, real mail clients, and real focus moves and announcement timing. The specs describe these behaviours, and WP4.2 checked that the markup doesn't contradict them.

## 2. Results at a glance (first pass)

| Test | Blocker | Major | Minor | Polish |
|---|---|---|---|---|
| Rig (default runs) | 0 | 0 | 564 (all on the known false-positive list) | – |
| Rig extra checks (text spacing, layout shift, drift, screenshots) | 1 | 2 | 6 | – |
| Keyboard and screen reader | 0 | 4 | 13 | – |
| Contrast | see §4 | | | |
| Copy | 0 | 6 | 12 | 13 |
| Coverage and buildability | 0 | 14 | 24 | – |

Passed outright on the first pass:
- No horizontal overflow at any width, including 200 % zoom.
- The mascot and `h1` never move between a page and its states.
- Every Tab stop has a name and a visible focus change, and nothing traps focus.
- Every accessible name starts with its visible label.
- Code inputs have one accessible name.
- Emails read as one string.
- axe reports 0 violations.
- No placeholder copy, no banned words, nothing workspace-related.
- All 73 boards match `canvas.json`.
- No broken board links or imports.

Known false positives that were discarded, with counts in each report:
- 36 px sidebar items and 32 px row buttons on desktop
- 24 px footer links on fine pointers
- inline links (exempt)
- buttons on the inert page behind overlays
- axe "needs review" items over layered backgrounds
- axe colour-contrast majors under `--forced-colors`: axe reads authored colours against forced backgrounds; screenshots of 14 or more boards confirm borders, badges, rings and marks survive

## 3. Findings and what was done

### Fixed by the PM (shared CSS, `edges.css` §9, and assembly)

| ID | Severity | Fix |
|---|---|---|
| RIG-01 | blocker | Block buttons wrap their label under text spacing (`white-space: normal`, `min-height` per size, padding from the height). Block `lg` side padding is now 16 px, so every block label fits 256 px on a 320 phone. Retest: no horizontal scroll at 320 with spacing; normal rendering is unchanged (button heights identical). Two labels still too long went to WP3.4 |
| RIG-02 | major | Same fix; "Use a different Microsoft account" now fits one line |
| RIG-08 | minor | Under forced colours, the filled primary uses the system Highlight pair, so it keeps its rank over outline buttons |
| RIG-09 | minor | Sidebar labels wrap instead of truncating under text spacing; at normal spacing nothing changes |
| RIG-04 | minor | Not a defect: `Account-sidebar` uses the spec sample address; the drift tool compared it with the two boards that show a changed address on purpose |
| CSS-REQUESTS 1–3, WP3.3-1, WP3.4-1/2, WP3.5-1 | – | Merged before Phase 4: forced-colours badge, inline slot icons, chip alignment, short local parts, textarea, lede address, page-alert gap |

### Settled by PM rulings (spec Part B)

| Rulings | Findings |
|---|---|
| B-6 … B-17 (copy) | COPY-01, 04, 05, 06, 08, 09, 11, 19, 20, 21, 27, 29, 30 |
| B-18 … B-26 (behaviour) | COV-01 (J7 dead end), 02 (support handoff), 04, 19, 20, 21, 27, 28, 35, 37 |
| B-27 … B-30 (accessibility) | A11Y-01, 03, 04, 05 |
| B-31 … B-33 | A11Y-16, RIG-07, the new boards |

### Fix round (designers)

Each designer's spec gets a "Fix log (Phase 4)" section that records every finding ID as fixed, won't-fix (with a reason) or moved. Summary:

| Owner | Findings | Status |
|---|---|---|
| WP3.2 Edge pages | 9 | 9 fixed. New boards: `Signedout-client`, `Limited-dark`, `Signin-limited-dark`. The time-limit warning is defined once (spec-edge §3.6). Fix log: spec-edge §15 |
| WP3.3 Settings gaps | 15 | 15 fixed. The `Security` base changed: two-line reservation, names wrap, and quoted names in labels. New board: `Security-load-error-dark`. Fix log: spec-settings §14 |
| WP3.4 Recovery | 27 | 26 fixed. COPY-31 is won’t-fix: the alert keeps its date, because it stays up for 7 days. New boards: `Verify-help`, `Recover-limited-dark`. `Recover` is now 1100 px tall. Fix log: spec-recovery §14; support handoff contract in §15 |
| WP3.5 Google and Microsoft | 16 | 16 fixed. New board: `Provider-not-linked` (P8). Fix log: spec-sso §18 |

## 4. Contrast

**0 failures** on all 73 boards in light and dark at 390 and 1440. That covers 2,542 text items, 32 placeholders, 482 control boundaries (inputs, outline buttons, switches, badge discs and glyphs) and 424 focus stops. Nothing is within 0.1 of a threshold. Every value in Part A §6.3 and `.design/edges/wp31/contrast-report.md` recalculates to the same ratio within 0.05.

| Lowest pairs | Ratio | Threshold |
|---|---|---|
| Light ok badge `#16a34a` on `#ffffff` (and its white glyph on the disc) | 3.30 | 3:1 (non-text) |
| Light outline edge `#86868b` on Mist `#f5f5f7` | 3.33 | 3:1 |
| Dark outline edge `#6e6e73` on dialog `#1d1d1f` | 3.32 | 3:1 |
| Dark outline edge `#6e6e73` on Mist `#161617` | 3.57 | 3:1 |
| Dark outline edge `#6e6e73` on `#000000` | 4.14 | 3:1 |

A hand probe on OTP slot edges gave 3.32 inactive and 4.31 active. On the four overlay boards, the "third Tab lands on no element" note is the static mock's end of document. In the app, the dialog keeps Tab inside it by making the page behind it inert (spec, AS §4.0 dialogs).

## 5. Retest (WP4.6, [`p4-regression.md`](p4-regression.md))

| Run (80 boards) | Blocker | Major | Minor (all known false positives) |
|---|---|---|---|
| Rig, light | 0 | 0 | 601 |
| Rig, forced-dark build | 0 | 0 | 601 |
| Text spacing (WCAG 1.4.12) at 320 and 1440, both builds | 0 | 0 | – |

- **Block labels at 320:** 127 buttons. The widest label is 242 px, and none is over 256 px or taller than 44 px.
- **Layout shift:** the mascot and `h1` never move. `Main` and `Security` against their copies show only the documented exceptions. RIG-05 is closed: `Security-linked` is now identical above the changed row.
- **Drift and links:** drift is limited to the intended `Account-sidebar` group. Icons: 0 bad. All 72 board links and every import resolve. The 7 new boards were checked at 320, 390 and 1440.
- **Finding statuses:** all 95 finding IDs from Phase 4 have a recorded status (fixed, won’t fix, ruled or moved). All 27 blockers and majors were verified with evidence on the board or in the spec. For minor and polish findings, 56 of 68 were verified or spot-checked, and 12 rest on the fix log.

The retest raised three items:

| ID | Severity | Item | Resolution |
|---|---|---|---|
| REG-01 | major | COV-14 was half applied: spec-edge still called the provider buttons links | Fixed by the PM. spec-edge §4 E7 and spec-recovery R1 now point to spec-sso §3.1, and both fix logs record it |
| REG-02 | minor | B-26’s Part A sync (COV-21, COV-27, COPY-19) wasn’t applied yet | Applied by the Part A sync (spec.md Part A Revision 2, with the "Limits and lifetimes" table in §3.5) |
| REG-03 | polish | This log was out of date | Updated |

**Phase 4 exit:** 0 blockers, 0 unexplained majors, and every finding is closed or explicitly ruled.
