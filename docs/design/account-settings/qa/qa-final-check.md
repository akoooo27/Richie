# Account settings canvas: final check (QA-V)

Independent verifier, 2026-11-05. Report only: no canvas file, spec or published artifact was changed. Sources: `.design/canvas/project/` (canvas version `1793830578-0c89`, 51 boards) against `spec.md` Revision 3. Items R-01 to R-23 of `qa-regression.md`.

## Method

- Build: `node .design/qa-rig/build.mjs .design/canvas/project .design/qa-out/verify-2 .design/qa-blobs/foundation` (51 boards, 0 warnings).
- Full rig run: `run.mjs --out .design/qa-out/verify-2 --port 9711 --variant both --workers 6` (results in `.design/qa-rig/results/verify-2/`).
- Probes (ports 9733, 9744, 9745; scripts `.design/qa-regress/tools/probe.mjs` plus ad hoc `/tmp/v2/p.mjs`, `/tmp/v2/q.mjs`): geometry per state and width, Chromium accessibility tree snapshots, real Tab order with `.is-focused` stripped, computed focus rings, status-string measurement with `.design/wp3-1/measure-status.mjs`, grep and a class audit.
- Leftover greps over all 51 boards: "sign-in code", "code from your email", "Verification code", "workspace", `role="alert"` on the locked state: all 0. "Check your connection and try again": 0 (the two "Check your connection." hits are the new short form). `grep` of the only `role="alert"` near it is `Profile-save-failed`, which is correct.
- The spec §6.2 sync had landed; R-08 and R-10 were checked against it once, at the end.
- Servers and browsers I started are closed. Nothing listens on 9700-9799.

## Result

**Tally: 22 Resolved, 1 Partially (R-23), 0 Not resolved.**

| ID | Result | Evidence |
|---|---|---|
| R-01 | Resolved | The five greps are empty. Email-code strings are on the boards: `Security.dc.html:28`, `Main.dc.html:128`, `Preferences.dc.html:111`, `Stepup.dc.html:22`, `Security-remove-passkey.dc.html:21`. Code input label is "Email code, 8 letters and numbers" in `Stepup-code` and `Export-request-code`. |
| R-02 | Resolved | `Stepup-code-locked`: `#code-error` has `tabindex="-1"`, no `role`, the board draws `is-focused` there, 0 `[role=alert]` and 0 `button.is-focused` on the board. |
| R-03 | Resolved | Real Tab to `#stepup-code` with `.is-focused` removed: `.ra-otp-slots` gets a 2 px solid `#0071e3` outline plus the 5 px halo (light, dark, 1280 and 390). `account.css:2648` and the forced-colours rule at `:3013`. The rig still lists one major `focus/no-visible-focus-indicator #stepup-code` on this board (it only compares the hidden input's own styles); false positive, the ring is on the sibling slot group. |
| R-04 | Resolved | `dlg:Stepup` against `Stepup-code`, `-error`, `-locked`: content height 386 at 1440 and 470 at 390 and 320, title top 281 / 398 / 194 on all four. The title no longer moves. |
| R-05 | Resolved | `Export-request.dc.html:24` uses the sidebar markup (sr full address plus `aria-hidden` local, `@`, domain). Accessibility tree: "paragraph: jamie.rivera@northwind.coffee" (one string). `Export-request-mobile` imports it. |
| R-06 | Resolved | Crop frame is square and follows `min(280, 100svh - 360)`, minimum 160: 280 at 1440x900 and 390x844, 180 at 1280x540 (slider visible, body 276/276), 160 at 844x390 and 320x480 (body scrolls 256/176, cue drawn). |
| R-07 | Resolved | `Main` grid: single column at a 619 px column; two columns at 620 (280 + 308); 647 gives 307 + 308; 660 gives 320 + 308; 680 and up gives 340 + 308. The form shrinks first, the preview holds 308. |
| R-08 | Resolved | §6.2 compared with `canvas.json` row by row: title, size, x and y match for 50 boards; `Deletion-pending` is the flagged off-grid row (-189, 10144), matching the canvas. Row y values 0, 1400, 2700, 4600, 5900, 7600, 8900, 10200 and the note texts and `maxW` match. Heading says "Boards (51)". |
| R-09 | Resolved | `Profile-photo-removed` is row 12 of §6.2 (x 15670, 1440x1000, F own sidebar); §5 names it; §6.1 lists the three own-sidebar boards. |
| R-10 | Resolved | Spec §6.2 title, `canvas.json` title and each board's `<title>` are identical for all 51 boards (scripted compare, 0 differences, including `Export-request-code` and `Deletion-pending`). |
| R-11 | Resolved | `Account-index.dc.html:28`: `Jamie Rivera<span class="ra-sr">, profile</span>`. Tree: link "Jamie Rivera , profile jamie.rivera@northwind.coffee". `Account-index-dark` imports it. |
| R-12 | Resolved | Lead decision. `Security.dc.html:43` link "Change email" with `aria-describedby="codes-email"`, the id exists on the address; present in all 8 Security-family boards that show it (via imports). Spec §4.3 and the Revision 3 note agree. |
| R-13 | Resolved | `Profile-photo-removed.dc.html:116`: "Undo" plus `<span class="ra-sr">, restore your photo</span>`. Tree: button "Undo , restore your photo". |
| R-14 | Resolved | `Profile-photo-crop.dc.html:25,29`: `aria-describedby="crop-desc crop-help"`; help text "With a keyboard, use the arrow keys to move your photo, plus and minus to zoom, and Home to center it." |
| R-15 | Resolved | All 7 named boards (and the two mobile wrappers through their imports) have `form.ra-dialog-form`; commits are `type="submit"` (Save photo, Remove passkey, Remove access, Delete account, Stepup). `Data-delete-confirm` no longer uses a hidden form. |
| R-16 | Resolved | `Export-request-code`: meta reserves 64 px at 320, 360 and 390 and 44 at 1440. Injecting a wrong-code error moves "Request download" by 0 px at 320, 375 and 1440. |
| R-17 | Resolved | `Connected-empty.dc.html:34`: `<h2 data-slot="empty-title" tabindex="-1">`. |
| R-18 | Resolved | `<nav aria-label="Account">` opens before "Back to Richie" and closes after Sign out in `Account-sidebar` and the three copies. Tree on Main, Profile-extreme, -name-saved, -photo-removed: Back, name, email, menu, Help and Sign out are all inside "navigation Account". |
| R-19 | Resolved | Lead decision. `Data.dc.html:57-59` has no foot or slot; spec §4.7 now says "There is no status slot" and gives the reason. |
| R-20 | Resolved | Lead decision. Boards: `fieldset` + sr `legend` + radios (tree: group "Appearance" with radios System, Light, Dark; group "Time format"). `#language-value` has `lang="en-US"`. Spec §4.5, §7.2 row 13 and the Revision 3 note agree; §8 row 13 maps to `RadioGroup`. |
| R-21 | Resolved | `Main.dc.html:89` "Hi! Can I change…"; `Security.dc.html:117` "Last active October 27." (spec line 801); `Data-deleted.dc.html:23` `#signin`. "Hi, can" and `#sign-in` have 0 hits. |
| R-22 | Resolved | Lead decision. Status slot reserves 20 px at a 680 column, 40 at 440 and 358, 60 at 288 (matches `account.css` and spec §1.5, §4.0, §7.2 row 16). 51 status strings (7 phrases x 3 failure patterns, plus all spec and board statics) measured at 480 px in Inter and the system font: all 1 line. The long "Email changed. We'll send codes to jamie@rivera.studio…" (2 lines) is no longer in the spec or any board; the short form is. |
| R-23 | Partially | Mechanics classes (`.ra-steps`, `.ra-step-ghost`, `.ra-step-busy`, `.ra-dialog-form`, `.ra-dialog-pane`, `.ra-form-alert`, `.ra-photo-*`, `.ra-skel-*`, `.ra-trunc`, `.ra-bar`) are described in §7.2 row 31 and §8; dead rules `.ra-p`, `.ra-strong`, `.ra-toast-info` are gone; row 28 no longer claims a chip on `Export-request-code`. Equal-width confirm footers are in place (`Security-remove-passkey`, `Connected-remove`: grid 142 + 142 at 1440). **Not in place:** the decision says the delete dialog also gets the equal-width footer. Spec §4.7 note, §7.2 row 18 and row 31 specify `.is-even` on it, but `.is-even` exists in no board and not in `account.css`; `Data-delete-confirm` renders right-aligned 96 + 156 at 1440 (equal at 390 only because the drawer stacks). See N-01. |

## New findings

| ID | Board / file | Severity | Finding | Evidence | Fix |
|---|---|---|---|---|---|
| N-01 | `Data-delete-confirm` (+ `-mobile` wrapper at 768 and up), spec §7.2 rows 18, 31, §4.7 | Minor | The delete dialog footer is right-aligned (Cancel 96 px, Delete account 156 px), not equal width. The spec and R-23 decision say equal halves via `.is-even`; the class does not exist on the canvas. The spec names a rule the boards do not have. | Probe: footer 520-920, `justify-content: flex-end`, buttons 96 and 156; `grep is-even` over boards and `account.css`: 0 hits; spec: 4 hits | Add `.is-even` (`flex: 1 1 0; min-width: max-content` from 768) to `account.css` and the footer, or drop it from the spec and keep the delete dialog right-aligned |
| N-02 | `account.css` against spec §7.2 and §8 | Minor | 23 classes still not named in the spec, none behaviour-bearing for a developer (for example `.ra-side-nav`, `.ra-otp-slots`, `.ra-step-footer`, `.ra-msgr-*`, `.ra-toast-*`, `.ra-link-solo`). Three are unused in any board: `.ra-alert-action`, `.ra-avatar-scrim`, `.ra-row-empty`. | Class audit script (121 classes) | Optional: delete the three dead rules |

No Blocker or Major regression noticed in passing. The drift script reports one group, `nav.ra-side-nav`, whose only difference is the `Profile-extreme` copy's own user data (expected).

## Final rig counts

Run on `.design/qa-out/verify-2`, 102 variants (51 fluid + 51 `.fixed`), light and dark, 10 viewports plus 200 % zoom:

| | Blocker | Major | Minor |
|---|---|---|---|
| Regression run (before) | 20 | 85 | 1483 |
| This run (`.design/qa-rig/results/verify-2/report.md`) | **20** | **77** | **1445** |

What is left, by cause:

- Blocker 20: horizontal overflow on `.fixed` 390 wrappers at 320 and 375. Fixed frame, false positive. Fluid variants: 0 blockers.
- Major 74 on `.fixed` mobile wrappers (Preferences-mobile 35, Security-mobile 23, Connected-mobile 8, Data-mobile 4, Profile-mobile 1) plus `Profile-extreme.fixed` 2: desktop shell inside a 390 div and the intended name clamp. False positives.
- Major 3 on fluid boards: `Profile-extreme` clipped sidebar name x2 (the intended 2-line clamp with ellipsis, §3.2; also counted once more on `.fixed`) and `Stepup-code-locked` `#stepup-code` no visible focus x1 (false positive, see R-03). Real majors: 0.
- Gone since the regression run: the four Data-delete-confirm footer overlaps and Connected-empty toast overlaps (rig) and the real `Stepup-code-locked` ring and Export-request chip issues.
- Minor 1445: target sizes under 44 px (accepted by §7.3 or probe artefacts), axe "needs review" colour contrast, inline links.
