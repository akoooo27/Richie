# Account settings canvas: QA A (visual and responsive)

Tester A, 2026-11-05. Scope: the Profile, shell and Sign-in and security rows (23 boards). Standards: [`../spec.md`](../spec.md), [`../../how-we-design.md`](../../how-we-design.md) §6, [`../../suiss-ui-guide.md`](../../suiss-ui-guide.md), WCAG 2.2 AA, and the identity-only scope rule.

**How this was tested**

- **Rig:** the built pages were copied from `.design/qa-out/consolidated/` to `.design/qa-out/qa-a/`. `run.mjs` ran with `--variant both` on port 9213, adding 844×390 to the default viewports. Raw output is in `.design/qa-rig/results/qa-a-visual/`: `report.md`, `report.json` and `shots/`. The rig reported 8 blockers, 347 majors and 600 minors; nearly all are false positives, listed in the false-positives section below.
- **Screenshots:** I looked at every board at 1440, 768, 390 and 320, in light and dark, at the 200 % zoom size (640×400), and with the focus shots. Overlays were also checked at 1280×540 and 844×390.
- **Extra passes:** stored in `.design/qa-rig/results/qa-a-visual/extra/`:
  - every non-dark state board with `.dark` added to `body` (the "dark for every state" harness);
  - forced colours on `Main`, `Security`, `Profile-name-error` and `Security-remove-passkey`;
  - DOM measurements: `positions.txt` (anchor positions per state), `dialog-geometry.txt` and `dialog-body-overflow.txt`.

All evidence paths below are relative to `.design/qa-rig/results/qa-a-visual/`.

## Summary

| Severity | Count |
|---|---|
| Blocker | 0 |
| Major | 6 |
| Minor | 11 |

**What passes:**

- **Dark mode:** surfaces and borders are correct on every board.
- **Focus:** the ring is visible on every tab stop, in light and dark.
- **Overflow:** no horizontal overflow at 320 px or at 200 % zoom on any fluid board.
- **Mobile targets:** they reach 44 px. Sidebar items are 44 on coarse pointers, `sm` buttons are 36 drawn with a 44 hit area, and rows are 52.
- **The 1440 layout:** the 680 px column, the shared `h1` position, the 62 px hairlines, the 40 px section rhythm and the radii all match spec §1.4–1.5.
- **Short heights:** dialogs cap at `100svh - 32px` and scroll inside.
- **Messenger header and bubble:** Signal `#0071e3` in both themes.

**What fails, in short:**

- Several state boards move the content below them, against spec §1.5 rule 8.
- The change-email dialog moves its footer between steps.
- The preview copy and its live-update story don’t match the spec.
- Long-email handling in the sidebar contradicts §4.2.

## Findings

| ID | Board(s) | Width / theme | Severity | What’s wrong | Expected (spec / standard) | Evidence | Suggested fix |
|---|---|---|---|---|---|---|---|
| A-01 | `Profile-name-error`, `Profile-name-error-dark`, `Profile-save-failed` (and the undrawn dirty state) | All widths, both themes | Major | The name action row isn’t held at 36 px. When Cancel and Save show, the empty `p.ra-status` (`flex: 1 1 240px`) wraps under the buttons, so `.ra-form-actions` is 64 px tall; with the save-failed alert it is 84 px. Content below moves. **1440:** the Email `h2` moves from 620 to 664 (error) and to 656 (failed). **390:** the preview moves from 612 to 668 and to 660, and the Email `h2` from 1092 to 1148 and to 1140. The first-name error adds another 28 px on top. | §1.5 rule 8: the name action row (36 px) is always in the layout, so a state change never moves the content below it. §4.2: "Action row (reserved 36 px)" | `extra/positions.txt`; `shots/Main.fixed/1440x900-light.png` against `shots/Profile-name-error.fixed/1440x900-light.png` and `shots/Profile-save-failed.fixed/1440x900-light.png` | Keep the status in the same 36 px row as the buttons (`flex-basis: 0`, or drop the empty status while the buttons show). Reserve a fixed 2-line slot under the row for the save-failed alert in every state. |
| A-02 | `Security-passkey-failed` (all widths), `Security-passkey-waiting` (768) | 1440, 768, 390, 320; both themes | Major | The passkeys status line pushes the next sections down. **Failed:** the alert can’t fit beside "Add a passkey", so it wraps onto a new row. The foot grows from 36 to 68 px at 1440, from 68 to 88 at 390 and from 68 to 108 at 320, and "Where you’re signed in" moves 32, 20 and 40 px. **Waiting at 768:** the foot grows from 36 to 68, a 32 px move. **Related:** below a 480 px column the empty status already sits on its own row, so the gap from a section foot to the next `h2` is about 88 px. Elsewhere it is 40–56. | §1.5 rule 8: section status lines are reserved (min-height 20) so a state change never moves the content below. §4.0: 40 px between sections | `extra/positions.txt`; `shots/Security-passkey-failed/390x844-light.png`; `extra/Security-passkey-failed-1440x900-darkclass.png`; `shots/Security-mobile.fixed/390x844-light.png` | Put the status line on its own row under the section action at every width, with room for 2 lines (about 40 px). Keep the section-to-section gap constant. |
| A-03 | `Profile-email-change` → `Profile-email-code` | ≥768 (1440×900, 1280×540, 844×390) | Major | The dialog is centred vertically, and step 2 is 72 px taller than step 1. Between the steps the title moves up 36 px and the footer moves down 36 px. **1440×900:** title from 329 to 293, footer from 527 to 563. **1280×540:** title from 149 to 113, footer from 347 to 383. The mobile drawer keeps the footer fixed, but its title still moves 68 px. | §4.2: "one dialog with two steps; the title changes per step and the footer stays in place". §4.4 states the same principle for step-up | `extra/dialog-geometry.txt`; `shots/Profile-email-change.fixed/1440x900-light.png`; `shots/Profile-email-code.fixed/1440x900-light.png` | Anchor the desktop dialog from the top (a fixed `top` offset with no −50 % translate) and give both steps the same body min-height. |
| A-04 | `Main` and every board that imports it (`Profile-*`, the 3 overlays); `Profile-extreme` | All widths, both themes | Major | **Copy:** the preview conversation isn’t the spec copy. The customer asks "Hi! Do you ship whole beans to Canada?" and Jamie replies "We do! Orders to Canada usually arrive in 4 to 6 business days." **Scope:** a coffee-shipping exchange ties the preview to one business: Northwind, the user’s `northwind.coffee` domain, which is a workspace context. **Screen-reader text:** it also differs: "a customer’s conversation in the messenger." | §4.2 preview table: "Hi! Can I change the delivery address on my order?" and "Of course. Send me the new address and I’ll update it right away." sr text: "Preview: a customer conversation in a Richie messenger. …" §1.6 principle 2 and §4.2 "Neutral by design" | `shots/Main.fixed/1440x900-light.png`; `canvas/project/Main.dc.html` (the messenger body) | Paste the spec copy into `Main`, `Profile-name-*`, `Profile-save-failed` and `Profile-extreme`, including the sr paragraph. |
| A-05 | `Profile-name-saved`, `Profile-save-failed` | All | Major | **The states show no edit.** Both boards keep "Jamie / Rivera" in the fields, the sidebar and the preview. `Profile-save-failed` shows Cancel and Save, and `Profile-name-saved` says "Name saved.", but nothing has changed. **The live update is never drawn.** No board shows the preview reacting to an unsaved name (dirty or failed) or the sidebar updating after a save. That live update is the page’s signature behaviour. | §4.2 name states: Dirty and Failed show the new name in the preview. Saved: "The sidebar name updates". §1.6 principle 1 | `shots/Profile-name-saved.fixed/1440x900-light.png`; `shots/Profile-save-failed.fixed/1440x900-light.png` | Give both boards an edited first name, for example "Jay". Failed: the field and the preview show "Jay" and the sidebar keeps "Jamie Rivera". Saved: the field, the preview and the sidebar all show the new name. |
| A-06 | `Profile-extreme` | ≥768, both themes | Major | The sidebar email reads "m…@northwind-specialty-coffe…". The domain is truncated too, and the local part is cut to a single faded glyph, so neither half is recognisable. The domain is about 300 px at 14 px, but the sidebar text box is 216 px, so the spec rule can’t be met on one line. The developer gets conflicting instructions. | §4.2 extreme: "in the sidebar the local part truncates and `@northwind-specialty-coffee-roasters.co.uk` stays whole". §3.2: the local part truncates first and the domain stays visible | `extra/Profile-extreme-sidebar-email-3x.png`; `shots/Profile-extreme.fixed/1440x900-light.png` | Decide in the spec. Either let the sidebar email wrap onto 2 lines ("local@" / "domain", the `ra-email-wrap` pattern) when the domain alone overflows, or amend §4.2 to allow domain truncation, and redraw. |
| A-07 | `Security-loading` | 768, 390, 320 | Minor | Below a 480 px column, skeleton rows leave out the wrapped action line, so they are shorter than the loaded rows. Email codes is 76 px against 144, and passkeys 96 against 144. When data arrives, "Where you’re signed in" jumps from 636 to 848 at 768 and from 648 to 892 at 390. Some jump is expected, because the foot buttons are hidden by spec. | §4.3: skeleton rows "shaped like its real rows". §7.2 row 22: "so the row height matches the real row" | `extra/positions.txt`; `shots/Security-loading/390x844-light.png` | In `@container (max-width: 479px)`, add a 36 px pill skeleton on the wrapped action line for rows that have actions. |
| A-08 | `Main`, `Profile-mobile` against `Profile-extreme` | 320 (and below about 380) | Minor | At 320, "Remove photo" wraps under "Change photo", so the photo row is about 86 px instead of 64. With no photo (`Profile-extreme`, "Upload photo" only) the form starts 22 px higher: the First name label sits at 332 instead of 354. Removing the photo at 320 therefore moves the whole form. A CSS comment claims this is rule-8 safe. | §1.5 rule 8 (a state change never moves the content below); §4.2 photo row | `shots/Main/320x640-light.png`; `shots/Profile-extreme/320x640-light.png`; `extra/positions.txt` | Keep the two buttons on one line at 320 (tighter gap, or put Remove photo under the hint), or reserve the wrapped height in both states. |
| A-09 | `Security`, `Security-dark` and its state copies | ≥480 px column (1440, 640) | Minor | The trailing plain "Change email" sits 13 px inside the actions edge, because its label is inset by the `sm` padding. Its right edge is 1159 against 1172 for the outline buttons in the rows below at 1440, and 595 against 607 at 640. The end-aligned action column is ragged. | §1.5 rule 3: actions are end-aligned | `shots/Security.fixed/1440x900-light.png`; `shots/Security/zoom200-1280x800-light.png` | Mirror the existing below-480 pull-back: `margin-inline-end: -12px` on a trailing plain `sm` row button. |
| A-10 | `Main` (two-column breakpoint) | 640 (200 % of 1280) and any 600–647 px column | Minor | Two columns start at a 600 px column. At 608 px the stage is 236 px and the messenger 204 px, against the 276 px design width. The reply wraps to 5 lines and the subline to 2. | §1.4: form 340 + gap 32 + stage 308 (messenger 276) | `shots/Main/zoom200-1280x800-light.png`; `extra/dialog-geometry.txt` (last lines) | Switch to two columns at a column of 648 px or more, or let the form column shrink before the stage does. |
| A-11 | `Security-remove-passkey` | ≥768, both themes | Minor | The confirm footer buttons have unequal widths: Cancel is about 118 px and Remove passkey about 165 px, because the footer is a flex row with `min-width: max-content`. | §7.2 row 19: "footer a 2-column grid". The suiss `AlertDialogFooter` uses equal columns | `shots/Security-remove-passkey.fixed/1440x900-light.png` | Use `grid-template-columns: 1fr 1fr`, and fall back to a stack only when a label doesn’t fit. |
| A-12 | `Profile-email-code` | All widths | Minor | The dialog body scrolls by 3 px (`scrollHeight` 179 against `clientHeight` 176 at 1440, 173 against 172 at 390). The 44 px `::after` hit area of the plain "Use a different email" button reaches outside the `overflow-y: auto` body. On classic-scrollbar platforms (Windows) a scrollbar appears in a dialog that has plenty of room, and the hit area is clipped to 42 px (39 px at 844×390). | §7.2 row 18 (the body scrolls only when it must); §7.3: plain inline buttons get a 44 px hit area | `extra/dialog-body-overflow.txt` (removing the `::after` gives 176/176); rig target finding "Use a different email 159x42" | Give `.ra-dialog-body` at least 8 px of bottom padding (`margin` compensated), or keep hit-area pseudo-elements inside scroll containers. |
| A-13 | `Profile-photo-crop` | 320–390 (below 768) | Minor | The Zoom slider is 32 px tall at phone widths, the only control under 44 px there. WCAG AA (24 px) still passes. | §7.3: below 768 or `pointer: coarse`, every control’s hit area is at least 44 × 44 | `shots/Profile-photo-crop/390x844-light.png`; rig target finding "Zoom 342x32" | Set the slider to 44 px tall under `(max-width: 767px), (pointer: coarse)`. The track stays 4 px. |
| A-14 | `Profile-photo-crop` | 844×390, 1280×540 | Minor | At short heights the body scrolls, as the spec asks. But the crop frame is cut mid-circle, the Zoom slider is fully out of view, and nothing shows that more content follows. On a touch screen the crop frame takes drag gestures, so scrolling to Zoom is awkward. | §3.2 short heights (the body scrolls, the title and footer stay visible) | `shots/Profile-photo-crop/844x390-light.png`; `shots/Profile-photo-crop/1280x540-light.png` | Add scroll-edge shadows to the dialog body. Better, size the crop frame with `min(280px, available height)` at short heights; that needs a spec call, because §3.2 says nothing changes size. |
| A-15 | `Main` and all Profile boards (the messenger) | All | Minor | Messenger metrics drift from the recipe: the body has 14 / 14 / 8 padding with a 12 px gap, and bubbles are capped at `min(250px, 88%)`. | §7.2 row 25: body padding 12 14, gap 8; customer bubble max 80 % | `canvas/project/account.css` (`.ra-msgr-body`, `[data-slot=bubble]`) | Align the CSS with §7.2 row 25, or update the spec if the new values are deliberate. |
| A-16 | `Main`, `Security` (sidebar) | Forced colours | Minor | The hairline under "Back to Richie" (`.ra-side-rule`, a background-painted div) disappears in forced-colours mode, so the back link runs straight into the account header. The footer rule survives, because it is a border. | §7.7: shapes survive in forced colours | `extra/Main-1440x900-forced.png` | Draw the rule as `border-top: 1px solid var(--sidebar-border)` instead of a background. |
| A-17 | Systemic: `account.css` | n/a | Minor | **Workspace leftovers** ship in the shared stylesheet that developers will copy:<br>- `[data-page=workspaces] [data-nav=workspaces]` (lines 458 and 2743);<br>- the `.ra-count` invitation pill (1233, 2672);<br>- `.ra-msgr-mark`, the workspace logo (2003);<br>- the comments "Back to the workspace" (374) and "workspace names" (694).<br>**Spec:** §10.3 still lists the `buildings` icon. No board renders any of these. | Scope rule: identity pages only (spec revision note; §1.6 principle 2) | `canvas/project/account.css` | Delete the selectors, the classes and the comments, and remove `buildings` from §10.3. |

## Rig false positives (verified, ignored)

1. **Horizontal-overflow blockers** on the `.fixed` variants of the 390 px wrappers (`Profile-mobile`, `Security-mobile`, `Security-remove-passkey-mobile`, `Account-index-dark`) at 320 and 375. The fixed wrapper is 390 px by design. The fluid variants have no overflow at any width.
2. **Clipping and overlap on `.fixed` mobile wrappers at viewports of 768 and up.** Media queries follow the viewport, so the desktop shell renders inside a 390 px div. On the canvas the frame is 390 px wide. At 390×844, the fixed variants render the mobile layout correctly.
3. **Text overlap on overlay boards** (`Profile-photo-crop`, `Profile-email-change`, `Profile-email-code`, `Security-remove-passkey`, `Security-remove-passkey-mobile`) is inert page text under the scrim colliding with dialog text.
4. **"No visible focus indicator"** on New email, the OTP input and Cancel. These elements already carry the canvas-only `.is-focused` ring, so focusing them changes nothing. The focus screenshots show the ring. The OTP case is the hidden real input drawn by its slots.
5. **axe colour-contrast "needs review"** on overlay boards is text under the 50 % scrim.
6. **Clipped text and overlap on truncated emails:**
   - `Account-index` at 320: the email local part, which is intended truncation;
   - `Profile-extreme`: the sidebar name clamp and the email spans;
   - `Profile-extreme` at 768 and 1024: "@northwind…" over "MF". That is the rect of text clipped by `overflow: hidden`, and it doesn’t render.

   I checked these at 3×: `extra/Account-index-320-email-3x.png`, `extra/Profile-extreme-sidebar-email-3x.png`. The sidebar domain truncation itself is a real issue, A-06.
7. **Targets:**
   - 36 px sidebar items and 32 px row buttons on fine pointers.
   - `Account-sidebar` is flagged at every viewport because the rig stretches the 256 px part to full width. At its board size, 256×900, it renders correctly.
   - Targets under 44 px on overlay boards belong to inert page buttons behind the scrim (Change photo, Rename, Sign out and so on).
8. **Black band under the shell** on `Profile-name-error-dark.fixed` and `Security-dark.fixed` at 1440×900. The viewport is shorter than the board (1000 and 1500). At board size the shell fills the frame.

## Per-board result

Fail means at least one Major applies, including one inherited through an import.

| Board | Result | Findings |
|---|---|---|
| `Main` | Fail | A-04, A-08, A-10, A-15 |
| `Profile-mobile` | Fail | A-04 (inherited), A-08 |
| `Profile-dark` | Fail | A-04 (inherited); dark rendering clean |
| `Profile-name-saved` | Fail | A-04, A-05 |
| `Profile-name-error` | Fail | A-01, A-04 |
| `Profile-name-error-dark` | Fail | A-01 (inherited); dark error tokens correct |
| `Profile-save-failed` | Fail | A-01, A-04, A-05 |
| `Profile-photo-crop` | Pass with minors | A-13, A-14 |
| `Profile-email-change` | Fail | A-03 |
| `Profile-email-code` | Fail | A-03, A-12 |
| `Profile-extreme` | Fail | A-04, A-06, A-08 |
| `Account-sidebar` | Pass with minors | A-16, A-17 |
| `Account-index` | Pass | |
| `Account-index-dark` | Pass | |
| `Security` | Pass with minors | A-09, A-16 |
| `Security-mobile` | Pass with minors | A-02 rhythm gap (inherited) |
| `Security-dark` | Pass with minors | A-09 |
| `Security-loading` | Pass with minors | A-07 |
| `Security-no-passkeys` | Pass | |
| `Security-passkey-waiting` | Fail | A-02 (768) |
| `Security-passkey-failed` | Fail | A-02 |
| `Security-remove-passkey` | Pass with minors | A-11 |
| `Security-remove-passkey-mobile` | Pass | |

## Systemic notes outside my lane (for testers B and C)

- **Missing boards:** `Data-delete-confirm.dc.html` and `Data-deleted.dc.html` are in `canvas.json` (and spec §6.2), but neither the project folder nor the consolidated build has them. 42 of the 44 boards were built.
- **Board title:** `Profile-name-error.dc.html` has `<title>Error: Profile – Richie</title>`, the app’s document title, instead of the board title "Profile, first name missing".
- **Board heights:** the spec §6.2 heights are out of date against `canvas.json`. For example, `Main` is 1000 in the spec and 900 on the canvas, `Security` 1700 and 1500, and `Security-no-passkeys` 1400 and 1000. The content fits the canvas sizes, so only the spec table is stale.
