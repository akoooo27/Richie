# Account settings canvas: QA B (visual and responsive)

Tester B, 2026-11-05. Scope: the Step-up, Preferences, Connected apps and Data rows (19 boards). `Data-delete-confirm` and `Data-deleted` are on hold, so they are not covered.

Standards: [`../spec.md`](../spec.md), [how-we-design §6](../../how-we-design.md), [suiss-ui-guide](../../suiss-ui-guide.md), WCAG 2.2 AA, and the Duende email-code facts: 8 characters of Crockford Base32, 4 + 4 slots, 5-minute lifetime, 4 tries, at least 60 s between sends.

## Summary

| Severity | Count |
|---|---|
| Blocker | 0 |
| Major | 3 |
| Minor | 11 |

The rig found no axe violations, no console errors, and no horizontal overflow on any fluid board at 320 px or at 200 % zoom. Every rig blocker, and every rig major except the ones listed below, turned out to be a known false positive (see the false-positives section).

The real problems:

- The code dialog moves when its state changes at narrow widths.
- The locked code state has no focus target.
- The Data page's delete copy contradicts the spec.

## Method

- **Copy and rig run.** The built pages were copied from `.design/qa-out/consolidated/` to `.design/qa-out/qa-b/`. The rig was run as:
  `node run.mjs --out ../qa-out/qa-b --port 9347 --boards <19 boards> --variant both --results ../qa-out/qa-b/results`
- **Rig coverage.** Viewports 320×640, 375×812, 390×844, 768×1024, 1024×768, 1280×800, 1440×900, 1920×1080, 1280×540 and 200 % zoom, each in light and dark.
- **Extra checks.** Scripts are in `.design/qa-out/qa-b-tools/` and their output is in `.design/qa-out/qa-b/results/extra/`.
  - Short heights 844×390 and 320×480 on the overlays.
  - Harness dark (`body.dark`) for state boards that have no drawn dark board.
  - Forced colours.
  - Coarse pointer at 1024×768.
  - Real keyboard focus captured by tabbing, with the canvas-only `.is-focused` class removed.
  - Geometry measurements: dialog and title position per state, hit areas, dialog-body scroll.
- **Screenshots reviewed** for every board at 1440, 768, 390 and 320 in both themes, plus 1280×540 and 200 % zoom for the overlays.
- **Comparison.** Section spacing and rhythm were compared with the `Main` and `Security` shots in `.design/qa-out/consolidated/results/shots/`.

Evidence paths below are relative to `.design/qa-out/qa-b/results/`.

## Findings

| ID | Board(s) | Width / theme | Severity | What’s wrong | Expected (§) | Evidence | Suggested fix |
|---|---|---|---|---|---|---|---|
| B-01 | `Stepup-code-error`, `Stepup-code-error-dark` | ≤ ~340 px (320 measured), both themes | **Major** | **The drawer grows when the error appears.** "That code isn’t right. You have 3 tries left." wraps to 2 lines. The meta area reserves only 44 px (2 lines) but now needs 64 px, so the drawer grows 20 px. Because it is bottom-anchored, the title moves up 20 px (y 214 → 194) and the slots move up 20 px (332 → 312), under the user’s thumb, between `Stepup-code` and `Stepup-code-error`. **The warning icon is detached.** It sits at the left edge (x 24) while the 2-line text is centred. | The code meta area is always reserved, so a state change never moves content (§1.5.8). Changing state never moves the title or the buttons (§4.4). | `shots/Stepup-code/320x640-light.png` vs `shots/Stepup-code-error/320x640-light.png`, and the same in `Stepup-code-error-dark/` | Reserve 3 lines (64 px) for `.ra-code-meta` when the container is under 360 px. Put the icon inline in a centred, `text-wrap: balance` paragraph so it stays with the first word. |
| B-02 | `Stepup-code-locked` | all widths | **Major** | **Focus is lost in the locked state.** The code input gets `disabled` on the 4th wrong try. That drops focus to `<body>` at the moment the error appears. The board marks no focus target (no `.is-focused`; the first Tab stop is "Use a passkey instead"), and §7.4 doesn’t name one. Developers would copy `disabled` and lose the user’s place in the dialog. | No focus loss: §4.0 bans `disabled` because it drops focus, and §7.4 lists a focus target for each dialog state. | `shots/Stepup-code-locked/1440x900-light.png`; keyboard order in `report.md` (Stepup-code-locked) | Move focus to the countdown commit button (it is `aria-disabled`, so it stays focusable) or to Cancel. Draw `.is-focused` on that button and add the rule to §4.4 and §7.4. |
| B-03 | `Data`, `Data-mobile`, `Data-dark`, `Data-ready` | all | **Major** | **The delete copy contradicts the spec.** The "Delete your account" description reads "We’ll delete your account 30 days after you ask. Until then, you can sign in to cancel." It promises a cancellable grace period. | §4.7: "Deleting your account signs you out everywhere and removes your profile, passkeys and connected apps." This contradicts §9 Q3 (no restore) and the §4.7 dialog line "You can’t undo this." | `shots/Data/1440x900-light.png`, `shots/Data-ready/1440x900-light.png` | Tie this to the pending deletion decision: either revert to the §4.7 copy, or update §4.7, §4.8 and §9 Q3 first. The board and spec must agree before handoff. |
| B-04 | `Stepup-code-locked` | 320–375 px; desktop | Minor | **Mobile.** The locked description is one line longer than the code-state description (3 lines vs 2). The drawer grows 20 px and the title moves up 20 px (375 px: y 406 → 386; 360 px: 334 → 314). **Desktop.** The commit label changes from "Confirm" (104 px) to "Send a new code in 0:41" (225 px), so Cancel jumps 121 px left. | Changing state never moves the title or the buttons (§4.4). | `shots/Stepup-code/375x812-light.png` vs `shots/Stepup-code-locked/375x812-light.png`; `shots/Stepup-code-locked/1440x900-light.png` | Give the step-up description a 3-line min-height below 400 px, and give the commit button a min-width equal to its widest label. Or relax the §4.4 wording. |
| B-05 | `Data`, `Data-mobile`, `Data-dark`, `Data-ready` | all | Minor | **The download copy drops spec text.** The description reads "Your profile, sign-in history, preferences and connected apps, as JSON files in a .zip." It leaves out "passkeys, sessions" and the sentence "It contains account data only." | Copy in §4.7 | `shots/Data/1440x900-light.png` | Use the §4.7 sentence verbatim. |
| B-06 | `Preferences`, `-mobile`, `-dark`, `-offline` | < 768 and coarse pointer (also measured at 1024×768 with touch) | Minor | **Some touch targets fall short of 44 px.** Segmented items (System/Light/Dark, 1:30 PM/13:30) and the Language picker measure **42 px** of hit height (items are 32 tall with an `::after` of −6; the picker is 36 with −4, and the probe shows about 2 px lost). Switches (44) and `sm` buttons (46) pass. AA (24 px) passes. | Below 768 px or on a coarse pointer, every hit area is at least 44 × 44 (§7.3). | `report.md`, target-size section for Preferences; probe output in `.design/qa-out/qa-b-tools/hit.js` | Use `::after { inset: -8px 0 }` on `toggle-group-item` and `select-trigger` at that breakpoint. |
| B-07 | `Stepup-code`, `-error`, `-error-dark`, `-locked`, `Stepup-mobile` | all | Minor | **The dialog body always scrolls by 3 px.** The `::after` hit area of the plain "Use a passkey instead" button (−8 px) runs past `.ra-dialog-body`’s 4 px padding. The body is scrollable even when everything fits (scrollHeight 179 vs clientHeight 176 at 1440×900). Windows, or macOS with "always show scrollbars", would draw a scrollbar, and the wheel jiggles the body 3 px. | No scrolling at sizes where the content fits (§3.2) | `node measure.mjs … ovf.js` output; `extra/Stepup-code-1440x900-light-sb-clip.png` (mac overlay scrollbars hide it) | Pad the bottom of `.ra-dialog-body` by 8 px (with a matching −8 px margin), or keep the pseudo hit area inside the padding. |
| B-08 | `Stepup-code`, `-error`, `-locked` | 844×390; 640×400 (200 %) | Minor | **No cue that the dialog body scrolls.** The header and footer stay in place as specified, but nothing shows that the body scrolls. At 844×390, "Use a passkey instead" is cut in half at the fold. At 200 % zoom in the locked state it is hidden entirely below "That was the last try for this code." | The dialog scrolls inside (§3.2). The alternative should stay discoverable. | `extra/Stepup-code-error-844x390-light.png`, `shots/Stepup-code-locked/zoom200-1280x800-light.png` | Show a hairline or fade on the footer edge when the body overflows. |
| B-09 | `Stepup` | 320 px | Minor | **One-word orphan.** The note "After this, we won’t ask again for 10 minutes." leaves "minutes." alone on the second line. | Polish | `shots/Stepup/320x640-light.png` | Add `text-wrap: balance` to `.ra-detail` and to the status text in `.ra-slot`. |
| B-10 | `Preferences`, `Preferences-offline` (all widths) | all | Minor | **The error sits far from its control.** In "Emails from Richie" the status slot comes after the foot note. The offline error, "We couldn’t save that change because you’re offline.", lands about 235 px below the "Product news" switch it describes, and below unrelated text. Other sections put the status directly under their group. | The status line follows the group (§4.0) | `shots/Preferences-offline/1440x900-light.png` | Put the status slot first in the foot and the note after it. |
| B-11 | `Connected-remove` | ≥ 768 | Minor | **Unequal confirm buttons.** The footer buttons are 125 px and 159 px wide, because the footer uses `flex` with `min-width: max-content`. The suiss alert dialog uses an equal 2-column grid. In a simulated equal grid, "Remove access" still fits (140 of 142 px). The same CSS applies to every confirm. | A 2-column grid (§7.2 recipe 19; suiss guide §3.4) | `shots/Connected-remove/1440x900-light.png`, `extra/Connected-remove-suiss-grid-footer.png` | Use `grid-template-columns: 1fr 1fr` and keep the stacked fallback for long labels. |
| B-12 | `Stepup-code`, `-error`, `Stepup-mobile`; spec §4.4 | all | Minor | **The resend timer can enable one second early.** It starts at "0:59", and §4.4 says it "restarts at 0:59". If the countdown starts at 0:59, the button enables 59 s after a send. Duende allows a new send only after at least 60 s, so an early press can fail as "We couldn’t send a code." | Honour the 60 s minimum (Duende facts) | `shots/Stepup-code/1440x900-light.png` | Start at 1:00, or drive the timer from the server’s next-allowed time. Update §4.4. |
| B-13 | `Preferences` family (systemic, copy-only states) | 320 px | Minor | **The status line can push content down at 320 px.** It reserves one 20 px line, but status errors wrap to 2 lines at 320 (the offline error measured 40 px). On the board it is harmless because Emails is the last section. The specified "Save failed" message in "Appearance and language", "We couldn’t save that change. Check your connection and try again.", would push "Date and time" down 20 px or more. | Reserved space means content never moves (§1.5.8) | `extra/`: alert.js output (offline error 288×40 at 320) | Reserve 2 lines below a 480 px container, or overlay the error in the 40 px section gap. |
| B-14 | All boards (`account.css`, systemic) | n/a | Minor | **Workspace leftovers in the stylesheet.** `[data-page=workspaces] [data-nav=workspaces]` remains (lines 458 and 2743), along with the comments "Back to the workspace" (374) and "(workspace names)" (694). Nothing renders, but developers porting the CSS would carry workspace hooks. | Identity-only scope (§1.6 principle 2; user rule) | `.design/canvas/project/account.css` | Delete the selectors and reword the comments. |

## Verified rig false positives

| Rig report | Where | Why it’s not a defect |
|---|---|---|
| **blocker** `overflow/horizontal-overflow` (scrollWidth 390 > 320 or 375) | `Stepup-mobile.fixed`, `Preferences-mobile.fixed`, `Connected-mobile.fixed`, `Data-mobile.fixed` at 320 and 375 | The `.fixed` variant keeps the 390 px wrapper by construction. The fluid variants have no overflow at 320 or at 200 % zoom. |
| **major** `clipped-text` ("Tallybird", "Slotwise", "Remove access") | `Connected-mobile.fixed` at 768 and wider | A 390 px frame rendered at a desktop viewport gets the sidebar layout. On the canvas the frame is the viewport (390), so this doesn’t happen. |
| **major** `overlap/text-overlap` (page text vs `#stepup-title`, `#stepup-desc`, `#remove-desc`) | Every Stepup board and `Connected-remove` at 320 | These are inert page rows behind the scrim and drawer. Screenshots show the drawer fully covering them. |
| **major** `focus/no-visible-focus-indicator` on `.is-focused` elements | `Stepup` ("Use a passkey"), `Connected-remove` (Cancel), `Preferences-offline` ("Product news") | Canvas-only rings. Tabbing with `.is-focused` removed shows the 2 px ring and halo (`extra/focus/`). |
| **major** `focus/no-visible-focus-indicator` on `#stepup-code` | All code boards | Hidden OTP input. The active slot draws the ring and caret in both themes (`extra/focus/focus-Stepup-code-1280x800-0-dark.png`). |
| **minor** `target-size` < 44 on the Security rows (Rename, Remove, Sign out, Add a passkey, Change email) | Step-up boards at 320–390 | These are inert buttons behind the overlay, so the probe hits the scrim. On `Connected` and `Data`, mobile `sm` buttons measure 46 px of hit height. |
| **minor** `target-size` 36 px sidebar items, 32 px row buttons | ≥ 768, fine pointer | Accepted by the spec (§7.3). With a coarse pointer, sidebar items are 44. |
| **minor** "Privacy Policy" 18 px | `Data*` | An inline link, exempt under WCAG 2.5.8. |
| **minor** `axe-review/color-contrast` (tiles, segment labels, OTP glyphs on the invalid tint, dialog descriptions, `strong` email) | various | Checked against the §7.1 table: all pass, and the tiles are `aria-hidden`. |
| Dark-scheme shots identical to light | Base, state and mobile boards | By spec §7.1, boards carry no `data-appearance` and always draw light. Drawn dark boards and harness dark (`body.dark`) were reviewed instead. No dark defects found. |

## Checked and fine

- **Dialog and drawer anchoring.** Correct at 320–1920 wide. At 1280×540, 844×390 and 320×480 the dialog caps at `100svh − 32` and the body scrolls while the title and footer stay. Action sheets fit at every height tested.
- **OTP at 320.** Slots are 27 × 44 with a 20 px glyph, 4 + 4 with a 22 px separator zone, and legible in both themes. The sample code `7KQ2M9XA` is valid Crockford Base32. Tries: 3 → locked on the 4th. The 5-minute expiry copy is present.
- **Segmented controls and switches** are readable in both themes and in forced colours (Highlight selection, `ButtonText` thumbs).
- **Offline alert.** It sits between the lede and the first section with 32 px on each side. It uses the suiss default Alert with a primary `WifiSlash` icon.
- **Alignment (§1.5).** One left edge (x 508 at 1440). The `h1` doesn’t move between pages (y 58). Hairlines sit at 62 px with media and 16 px without. Actions wrap at the text edge in columns under 480 px and are top-aligned on rows of 2 or more lines.
- **Shared tokens.** Type, weights and radii match the spec: groups, empty state and alert 18; drawer 28; slots 10; pills.
- **Toast.** Placed bottom-end with 16 px insets, and bottom-centre below 640 px.

## Systemic notes for the other testers

- **`Security` badge in forced colours.** The "This device" badge, visible behind the step-up boards, has a forced-colours outline that clips its first and last letters, because the badge has no padding. This belongs to the Security owner.
- **Reserved-height pattern.** B-01, B-04 and B-13 share one root cause. Reserved heights (meta 44, status 20, description) are sized for desktop line lengths, and at 320 px the specified copy is longer than the space reserved for it. Worth checking `Profile-email-code`, which has the same meta area.
- **Confirm footer.** B-11 applies to every confirm, including `Security-remove-passkey`.

## Per-board result

| Board | Result | Findings |
|---|---|---|
| `Stepup` | Pass | B-09 |
| `Stepup-mobile` | Pass | B-07, B-12 |
| `Stepup-code` | Pass | B-07, B-08, B-12 |
| `Stepup-code-error` | **Fail** | B-01; B-07, B-08, B-12 |
| `Stepup-code-error-dark` | **Fail** | B-01; B-07, B-08, B-12 |
| `Stepup-code-locked` | **Fail** | B-02; B-04, B-07, B-08 |
| `Preferences` | Pass | B-06, B-10, B-13 |
| `Preferences-mobile` | Pass | B-06, B-13 |
| `Preferences-dark` | Pass | B-06 |
| `Preferences-offline` | Pass | B-06, B-10, B-13 |
| `Connected` | Pass | none |
| `Connected-mobile` | Pass | none |
| `Connected-dark` | Pass | none |
| `Connected-remove` | Pass | B-11 |
| `Connected-empty` | Pass | none |
| `Data` | **Fail** | B-03; B-05 |
| `Data-mobile` | **Fail** | B-03; B-05 |
| `Data-dark` | **Fail** | B-03; B-05 |
| `Data-ready` | **Fail** | B-03; B-05 |

B-14 (CSS leftovers) applies to all boards and is not repeated in the table.
