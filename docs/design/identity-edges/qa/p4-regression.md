# WP4.6 regression retest (Phase 4, identity edges)

Tester: WP4.6. Canvas version 1793896016-b1a5 (local mirror `.design/edges/project/`), 80 boards, 2026-11-05. No board, stylesheet or spec was edited.

## 1. Run summary

| Run | Boards | Blockers | Majors | Minors | Discarded |
|---|---|---|---|---|---|
| 1. Light build, default viewports (320 to 1920, 200% zoom) x light and dark scheme (`qa/p46/light`) | 80 | 0 | 0 | 601 | 601 (all known false positives, below) |
| 2. Forced-dark build (`BOARD_THEME=dark`), `--schemes dark` (`qa/p46/dark`) | 80 | 0 | 0 | 601 | 601 (same classes and counts) |
| 3. Text spacing (WCAG 1.4.12) at 320 and 1440, both builds | 80 | 0 | 0 | 0 | 27 lines per build, all skip-link or sr-only span (see 2) |
| 4. Block button labels, 320 viewport, normal spacing, both builds | 80 | 0 | 0 | 0 | 127 block buttons, widest label 242 px, none over 256 px, none taller than 44 px |
| 5. Layout shift, 390 and 1440 | Security vs 14 states, Main vs 11 | 0 | 0 | 0 | see 3 |
| 6. `drift.mjs` | 1 shared group with drift | | | | RIG-04 (known, ruled not a defect) |
| 7. `check-icons.py` (icon dir first) | all | `bad: 0` | | | |
| 8. `href="X.dc.html"` | 72 hrefs, all `<dc-import>` names | 0 broken | | | |
| 9. New boards by screenshot (320, 390, 1440) | 7 | 0 | 0 | 0 | see 4 |

Discards in each rig run (601 minors, identical in both builds; the rig's counts per board are in each `report.md`):

| Class | Count |
|---|---|
| Sidebar items 36 px on fine pointers | 232 |
| Footer links 24 px (`.ri-foot`, `.ri-rec-mail-foot`, email footers) | 174 |
| Inline links under 24 px or multi-line inline links (WCAG 2.5.8 exempt) | 46 |
| 32 and 36 px row buttons, photo buttons, inert page behind overlays (Profile-email-*, Security-rename/-unlink/-remove-failed/-rename-error-mobile, Session-expired*) and block-less 40 px outline buttons on the edge pages | 110 |
| axe "needs review" colour-contrast (overlay pages, `#code`, `#link-code`, layered backgrounds; was 35 on 73 boards, now 39 on 80, the four extra are on the new dark limited and Verify-help boards) | 39 |
| Total | 601 |

Nothing is blocker or major in either run, so no non-false-positive finding remains from the rig. There is no `no-visible-focus-indicator`, no clipped or overlapping text and no horizontal overflow in any of the 80 boards at any width.

## 2. Text spacing

Script: the WP4.1 `ts.mjs`, pointed at `out-p46/` and `out-p46-dark/` (line-height 1.5, letter-spacing .12em, word-spacing .16em, paragraph spacing 2em; 320 and 1440). Both builds report the same 27 lines: 26 are "Skip to content" (visually hidden link, known) and 1 is Recover-declined at 320 "Contact support, opens in a new tab" (30 vs 44 px: the sr-only span, known). No hscroll, no clipped text, no ellipsis, and no button label outside its box. Previously failing boards (Recover-declined, Recover-limited, Provider-no-account, Provider-no-email, Signup-finish*, Security-passkey-added) are clean.

## 3. Layout shift

Method: Playwright against `out-p46/` (fluid), 390x844 and 1440x900; dx/dy of the mascot (`.ri-mark`), `h1`, first filled button, and for Security every `h2`, against the base board.

| Base | Result at 390 and 1440 |
|---|---|
| Main vs Main-dark, Main-mobile, Signin-redirecting, -offline, -offline-dark, -limited, -limited-dark, -remembered-provider | identical (mascot, h1, button) |
| Main vs Signin-provider-canceled / -error / -error-dark | mascot and h1 0; button +164/+228 at 390, +140/+180 at 1440: the documented load-time alert push (Part A 4.10) |
| Security vs Security-dark, -mobile, -linked, -remove-failed, -rename, -rename-error-mobile, -unlink | identical (h1 and every h2). Security-linked is now identical: RIG-05 is closed |
| Security vs Security-link-error | h1 0; the two h2 below the section +48 (1440) / +68 (390): the accepted two-line error push |
| Security vs Security-passkey-added | h1 0; h2 below +96 (1440) / +144 (390): one new row, expected |
| Security vs Security-passkey-limit | h1 0; h2 below +752 / +1224: ten rows, expected |
| Security vs Security-load-error and -dark | h1 0; h2 below -98 / -190: alert replaces the list, expected |
| Security vs Security-recovered | h1 0; alert above Email codes pushes it +148 / +244; later sections differ by design (1 passkey, 1 device, Google not linked, spec-recovery §3.12 and D-R9), so not a shift reference |
| Security vs Session-expired, -mobile | not a Security copy (Profile page behind a dialog); no h2s to compare |

No `h1` or mascot moves on any board. Nothing above a changed section moves.

## 4. New boards (screenshots at 320, 390, 1440)

Montages in the scratch dir (`.../scratchpad/p46/*-mont.png`); rig screenshots in `qa/p46/{light,dark}/shots/<board>/`.

| Board | Result |
|---|---|
| Signedout-client | Primary "Sign in again", outline "Back to Tallybird", note; no overflow at 320 |
| Limited-dark | "Try again in 12:00" muted, outline; note readable on black |
| Signin-limited-dark | "Send code in 41:12" with focus ring, slot line "This email has received 5 codes in an hour." |
| Security-load-error-dark | Alert "We couldn’t load your passkeys" + Try again, sidebar present at 1440, mobile header at 390/320 |
| Recover-limited-dark | "Send request at 9:15 AM", limit line, Contact support link; long fields scroll inside the input at 320 (normal) |
| Verify-help | Code slots, "Can’t get the code?" and "Not your email? Use a different one" |
| Provider-not-linked | Two-line h1, "Continue" + "Go to Sign-in and security", no overflow |

All seven render with the mascot, footer and layout of their siblings; none has a rig finding above minor.

## 5. Verification table

Severity is from the Phase 4 report. Status comes from the four Fix logs, test-log §3 and spec Part B. Verified: yes = evidence found on a board or in the spec text this run; spot = a partial or indirect check; no = status taken from the log only.

| ID | Severity | Status | Verified | Note |
|---|---|---|---|---|
| A11Y-01 | major | Fixed (B-27) | yes | Signin-limited, Limited: digits in `aria-hidden` span plus `ra-sr` ", available at 4:53 PM / 4:24 PM"; Recover-limited label has no digits |
| A11Y-02 | major | Fixed (WP3.3) | yes | Security-passkey-limit foot has `.ra-slot` (polite + alert paragraphs) then `.ra-note` with the limit line |
| A11Y-03 | major | Fixed (B-28) | yes | Signin-redirecting: "Taking you to Google…" in `ra-sr` inside the `role=status` paragraph; spec-edge E4/E8/E12 and spec-sso carry the rule |
| A11Y-04 | major | Fixed (B-29) | yes | spec-edge §3.6 time limit warning ("Still there? This page times out in 2:00." + Keep going, 20 more minutes); spec-recovery §15.3 table |
| A11Y-05 | minor | Moved to handoff (B-30) | no | Log and Part B only; no board change expected |
| A11Y-06 | minor | Fixed (WP3.2) | yes | Signin-limited: `id="slot-status"` and commit `aria-describedby="slot-status"` |
| A11Y-07 | minor | Fixed (WP3.4) | yes | spec-recovery D-R4 and R5 offline row: visible "send it", `aria-hidden` "it" + sr "your request"; copy-only |
| A11Y-08 | minor | Fixed (WP3.5) | yes | spec-sso §5.2/§6.1 `first-name-error`, `email-error`; Signup-finish draws `name-hint` in its slot row |
| A11Y-09 | minor | Fixed (WP3.4) | spot | Step titles in spec-recovery R2 (line 218); board `<title>`s are board names, so spec-only |
| A11Y-10 | minor | Fixed (WP3.4) | yes | Recover-approved: Cancel request is `<a data-slot="button" href="Recover-cancel.dc.html">` |
| A11Y-11 | minor | Fixed (WP3.4) | yes | Security-recovered: `<section data-slot="alert" aria-labelledby="recovered-title">` |
| A11Y-12 | minor | Fixed (WP3.5) | yes | spec-sso §3.3 line 150: label "Copied" at the button, "Reference copied." sr-only in the page slot |
| A11Y-13 | minor | Fixed (WP3.3) | yes | Security: `aria-label="Rename the “…” passkey"` (quoted) |
| A11Y-14 | minor | Fixed (WP3.3) | yes | Security-load-error: `role="alert"` on `.ri-set-alert-text`, Try again outside it |
| A11Y-15 | minor | Fixed (WP3.3) | no | Spec text (§1.4, §4.2) per log; not re-read |
| A11Y-16 | minor | Ruled B-31 | spot | B-31 present in Part B; applied in each domain spec per logs |
| A11Y-17 | minor | Fixed (WP3.5) | yes | Signin-remembered-provider: `form.ri-sso-forget` |
| COPY-01 | major | Fixed (B-6) | yes | "couldn’t reach" and "use a passkey because" gone from all four specs; "We couldn’t sign you in because you’re offline." in spec-edge, spec-recovery, spec-sso |
| COPY-02 | major | Fixed (WP3.3) | yes | spec-settings §5.2: "sign in now and remove it … Then sign out of any device"; old three-item sentence gone |
| COPY-03 | major | Fixed by removal (WP3.5) | yes | spec-sso §8.5 now points to spec-settings §5.2 |
| COPY-04 | major | Ruled B-7, applied | yes | "Check Sign-in and security" is the only button; "Review Sign-in and security" and "linked today" gone from the specs |
| COPY-05 | major | Ruled B-8, applied | yes | "Pacific Time" in spec-settings (7), spec-recovery (8), spec-sso (1); Email-recovery-alert board shows the zone |
| COPY-06 | major | Ruled B-9, applied | yes | "Your organization hasn’t approved Richie" in spec-settings and spec-sso; "365 admin" and "has to approve" gone |
| COPY-07 | minor | Fixed (WP3.4) | yes | Security-recovered: "Not linked. Works with any Google account." |
| COPY-08 | minor | Ruled B-10, applied | yes | Email-recovery-alert has "Cancel this request" |
| COPY-09 | minor | Fixed (B-11) | yes | Limited h1 "Too many attempts" |
| COPY-10 | minor | Fixed in substance (WP3.4) | no | Log only (noun kept for AT, visible noun dropped for slot width) |
| COPY-11 | minor | Fixed (B-12) | yes | Signin-limited: "This email has received 5 codes in an hour." |
| COPY-12 | minor | Fixed (WP3.4) | yes | Recover, Recover-account, Recover-review: "Richie support usually replies within 2 business days." |
| COPY-13 | minor | Fixed (WP3.3) | yes | Security-load-error: "We couldn’t load your passkeys" |
| COPY-14 | minor | Fixed (WP3.5) | yes | Provider-link lede "Confirm it’s yours to link Microsoft." |
| COPY-15 | minor | Fixed (WP3.2) | yes | Timeout board note "times out after 20 minutes" |
| COPY-16 | minor | Fixed (WP3.4) | spot | spec-recovery R2 line 243 "This took too long" (copy-only) |
| COPY-17 | minor | Fixed (WP3.5) | yes | Provider-no-account primary "Sign up with Google" |
| COPY-18 | minor | Fixed (WP3.4) | yes | Email-recovery-alert footer has Terms |
| COPY-19 | polish | Ruled B-26, NOT applied | no | Part A rows still stale (REG-02) |
| COPY-20 | polish | Ruled B-13, applied | spot | Per spec-edge log; board `<title>`s are board names |
| COPY-21 | polish | Ruled B-14, applied | yes | Error: "Go to Richie" |
| COPY-22 | polish | Fixed (WP3.2) | yes | Error-link lede "It may have expired or already been used." |
| COPY-23 | polish | Fixed (WP3.4) | yes | Recover-account lede "Enter the email address you can’t get anymore." |
| COPY-24 | polish | Fixed (WP3.3) | yes | spec-settings line 148 "Passkey added. That’s 10, the limit." |
| COPY-25 | polish | Fixed (WP3.3) | yes | Security-link-error note "…unlink it there first" |
| COPY-26 | polish | Fixed (WP3.4) | yes | Recover-approved title "Recovery request, approved, waiting 3 days" |
| COPY-27 | polish | Ruled B-15, applied | yes | Limited "Try again in 12:00"; spec-edge §3.4 |
| COPY-28 | polish | Fixed (WP3.5) | yes | spec-sso §8.5 sample time 9:31 AM |
| COPY-29 | polish | Ruled B-17, no change | spot | Ruling text in Part B |
| COPY-30 | polish | Ruled B-16, applied | yes | spec-settings line 43 and 162: relative dates stop at yesterday |
| COPY-31 | polish | Won’t fix (by design, WP3.4) | spot | Reason recorded in spec-recovery log (alert is a date, activity row is relative) |
| COV-01 | major | Ruled B-18, applied | yes | spec-settings Q-S1 and §3 (10-minute window after any sign-in); spec-recovery D-R16 (R1 routes to `/account/profile#email`) |
| COV-02 | major | Ruled B-19, applied | yes | spec-recovery §15.1 support handoff contract and topic list |
| COV-03 | major | Fixed (WP3.4) | yes | spec-recovery R1 `from=link` row; Verify-help and Recover wired |
| COV-04 | major | Ruled B-20, applied | yes | "until the first character is typed" in spec-recovery R4 and spec-sso P6 |
| COV-05 | major | Fixed (WP3.4) | yes | spec-recovery R2 open-request line with "open for up to 7 days" and Contact support |
| COV-06 | major | Fixed (WP3.4) | yes | Support links drawn on Recover, Recover-code, Recover-limited |
| COV-07 | major | Fixed (WP3.4) | yes | spec-recovery R2 no-account line "No Richie account uses this email."; no date |
| COV-08 | major | Fixed (WP3.3) | yes | spec-settings §9.2 "Which host owns what", `/signin-google` |
| COV-09 | major | Fixed (WP3.3/WP3.5) | yes | One email in spec-settings §5.2; spec-sso §8.5 points to it |
| COV-10 | major | Fixed by decision (WP3.5) | yes | Provider-no-account and Provider-no-email use `<a>` links; spec-sso line 342 "No status slot, by decision" |
| COV-11 | major | Fixed (WP3.5) | yes | New board Provider-not-linked drawn, h1 "You’re signed in, but Microsoft isn’t linked" |
| COV-12 | major | Fixed (WP3.4) | yes | New board Verify-help; Recover "Back to the code" links to Verify-help.dc.html |
| COV-13 | major | Fixed (WP3.2) | yes | New board Signedout-client shows "Back to Tallybird" |
| COV-14 | major | Partly fixed: WP3.5 half only | yes | spec-sso §3.1 is the mechanism; spec-edge line 351 still says "The provider buttons are links" and has no COV-14 log row (REG-01) |
| COV-15 | minor | Fixed (WP3.5) | no | Log only |
| COV-16 | minor | Fixed (WP3.4) | no | Log only |
| COV-17 | minor | Fixed (WP3.3) | no | Log only |
| COV-18 | minor | Fixed (WP3.3, B-9) | yes | Wording present in spec-settings §4.2 and Q-S9 |
| COV-19 | minor | Ruled B-21, applied | yes | Boards Limited-dark, Signin-limited-dark, Recover-limited-dark, Security-load-error-dark exist and render dark |
| COV-20 | minor | Ruled B-22 | spot | Ruling text present; this run carries 320/390/1440 renders of every board |
| COV-21 | minor | Ruled B-26, NOT applied | no | Part A has no "superseded by" notes (REG-02) |
| COV-22 | minor | Fixed (WP3.4) | yes | Forms in Recover-account, -email, -code, -details point to the next board |
| COV-23 | minor | Fixed (WP3.4) | yes | spec-recovery §15.5 present |
| COV-24 | minor | Fixed (WP3.4) | yes | spec-recovery §15.4 token table |
| COV-25 | minor | Fixed (WP3.5) | no | Log only |
| COV-26 | minor | Fixed (WP3.3) | yes | Profile-email-taken boards: "Send code in 41:12"; rig clean at 1440/390/320 |
| COV-27 | minor | Ruled B-26, NOT applied | no | No "limits and lifetimes" table in Part A §3.5 (REG-02) |
| COV-28 | minor | Ruled B-23, applied | yes | spec-sso §3.5 L6 counts failed provider sign-ins only |
| COV-29 | minor | Fixed (WP3.2) | yes | spec-edge E2 restore table; "We put back what you typed" row present |
| COV-30 | minor | Fixed (WP3.5) | spot | Provider-no-email second action reads "Sign in with your email instead" |
| COV-31 | minor | Fixed (WP3.4) | yes | "No passkey for Richie on this device?" in spec-recovery (2 hits) |
| COV-32 | minor | Fixed (WP3.5) | yes | spec-sso line 290: Cancel returns to the start page recorded in the challenge |
| COV-33 | minor | Fixed, measured, accepted (WP3.4) | no | Log only |
| COV-34 | minor | Fixed (WP3.2) | spot | Log only plus spec-edge E4 section present |
| COV-35 | minor | Ruled B-24, applied | yes | spec-recovery §15.2 Help article minimum content |
| COV-36 | minor | Fixed (B-29) | yes | spec-edge §3.6 sliding clock ("20 more minutes") |
| COV-37 | minor | Ruled B-25, applied | yes | spec-recovery line 581 and 912: signed-in "Can’t get the code?" offers account recovery |
| COV-38 | minor | Fixed (WP3.4) | no | Log only |
| RIG-01 | blocker | Fixed (PM CSS §9 + WP3.4 labels) | yes | Block labels measured at 320 in both builds: 127 block buttons, widest label 242 px, none over 256 or taller than 44; text spacing at 320 clean on Recover-declined and Recover-limited (only the sr-only span false positive) |
| RIG-02 | major | Fixed (PM CSS §9) | yes | Same measurement; Provider-no-account, Provider-no-email, Signup-finish* fit one line |
| RIG-03 | major | Fixed (WP3.3) | yes | `wrap-all` on 8 passkey/activity titles in Security-passkey-added, no `-webkit-line-clamp` left on them; text spacing at 320 and 1440 reports no clipping |
| RIG-04 | minor | Ruled: not a defect (PM) | yes | drift.mjs still reports Account-sidebar (sample address `jamie@rivera.studio`) as the only drift; matches the ruling |
| RIG-05 | minor | Fixed (WP3.3, different cause) | yes | Security-linked now has h1 and every h2 at the same y as Security at 390 and 1440 (identical) |
| RIG-06 | minor | Fixed, documented (WP3.4) | spot | Security-recovered still differs from Security by design (h2 +148 Passkeys at 1440); documented in spec-recovery §3.12 and D-R9 |
| RIG-07 | minor | Ruled B-32 | yes | B-32 text present in Part B; re-measured: h1 and mascot never move, only page-to-page buttons differ |
| RIG-08 | minor | Fixed (PM CSS) | yes | edges.css:556 forced-colors rule gives the filled primary Highlight/HighlightText (screenshot not retaken) |
| RIG-09 | minor | Fixed (PM CSS) | yes | Text spacing at 1440: no ellipsis reported on any sidebar board |

Counts: 95 IDs, all with a recorded status (RIG 9, A11Y 17, COPY 31, COV 38). The blocker and the 26 majors are 27 findings: 26 verified yes with evidence, 1 (COV-14) verified as only partly fixed. Of 68 minors and polish: 46 verified yes, 10 spot, 12 log only. Total verified yes or spot: 83 of 95. p4-contrast raised no CON IDs; its two observations (Tab 3 lands on `body` on four overlay boards; Part A vs hand probe) are the known static-mock end-of-document and need no action. COV-14, COV-21, COV-27 and COPY-19 do not hold (below).

## 6. New or remaining issues

| ID | Severity | Board(s) | Evidence | Owner |
|---|---|---|---|---|
| REG-01 | major (the open half of COV-14) | spec-edge E7 (§4, line 351), spec-recovery R1 | WP3.5 made spec-sso §3.1 "the one mechanism" for provider buttons (form submit, `fetch` + `location.assign`, "We couldn’t sign you in…") and says WP3.2 and WP3.4 align to it. spec-edge line 351 still says "The provider buttons are links. When `navigator.onLine` is false, the click is intercepted…", and spec-edge §15 has no COV-14 row. spec-recovery R1 gives the copy (line 207-208) but states no mechanism and has no COV-14 row. The contradiction the finding named (links vs form submit) is still in the specs. No board change needed | WP3.2 (E7 text and log row), WP3.4 (R1 pointer and log row) |
| REG-02 | minor (COV-21, COV-27, COPY-19) | spec.md Part A (§3.1, §3.4, §3.5, §5.2) | B-26 and test-log §3 say Part A is synced with a "superseded by" note on each changed row and a new "limits and lifetimes" table in §3.5. Part A (lines before 988) has no such notes (the only "superseded" hit is an unrelated CSS row) and no such table. Examples still stale: R1 row (line 268) lists entry points "A6, A5, footer Help article" without P6 and says Back to the code -> Verify; the R1 `from=link` variant is only in spec-recovery | PM |
| REG-03 | polish | qa/test-log.md | §1 says 73 boards and §3 shows all four designer rows "in progress", §5 "Pending WP4.6"; the fix logs are complete and the canvas has 80 boards. Update after this report | PM |

Known and not counted: drift.mjs reports Account-sidebar (RIG-04, ruled not a defect); forced-colours was not re-run (not in this brief); static-mock end-of-document Tab on overlay boards.

## 7. Verdict

Phase 4 exit criteria **do not yet hold, by two small spec-text items**: 0 blockers and 0 rig majors (met); every one of the 27 blocker and major findings is fixed with evidence except COV-14, whose WP3.2 and WP3.4 half is not in the specs (REG-01); and three minor or polish findings (COV-21, COV-27, COPY-19) were ruled by B-26 but Part A was not synced (REG-02). Close REG-01 and REG-02 and all three criteria hold.
